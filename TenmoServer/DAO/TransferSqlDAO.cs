using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {

        public TransferSqlDAO()
        {

        } 
        
        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        private readonly string connectionString;


        public int MakeTransfer(Transfer transfer)
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id,account_from, account_to, amount) " +
                    "VALUES ((SELECT transfer_type_id FROM transfer_types WHERE transfer_type_desc = @type), " +
                    "(SELECT transfer_status_id FROM transfer_statuses WHERE transfer_status_desc = @status), @from, @to, @amount)", conn);

                //string statusID = $"(SELECT transfer_status_id FROM transfer_statuses WHERE transfer_status_desc = @status)";
                //string typeID = $"(SELECT transfer_type_id FROM transfer_types WHERE transfer_type_desc = @type)";

                cmd.Parameters.AddWithValue("@type", transfer.Type);
                cmd.Parameters.AddWithValue("@status", transfer.Status);
                cmd.Parameters.AddWithValue("@from", transfer.FromUserID);
                cmd.Parameters.AddWithValue("@to", transfer.ToUserID);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                int returnedIdentity = Convert.ToInt32(cmd.ExecuteScalar());
                
                if (returnedIdentity > 0)
                {
                    transfer.ID = returnedIdentity;
                }
            }
            
            return transfer.ID; 
        }

        public Transfer GetTransferDetails(int transferID)
        {

            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT transfer_id,transfer_type_id, transfer_status_id,account_from, account_to, amount FROM transfers WHERE transfer_id = @transferID", conn); // Needs to be finished. 
                sqlCommand.Parameters.AddWithValue("@transferID", transferID);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    transfer = GetTransferFromReader(reader);                    
                }
            } 
            return transfer; 
        }
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                ID = Convert.ToInt32(reader["transfer_id"]),
                FromUserID = Convert.ToInt32(reader["account_from"]),
                ToUserID = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"]),
            };

            return t;
        }

        public List<Transfer> TransferHistory(int userID)
        {
            List<Transfer> transferList = new List<Transfer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT transfer_id, account_from, account_to, amount, users.username FROM transfers " +
                    "JOIN transfer_statuses ON transfer_statuses.transfer_status_id = transfers.transfer_status_id " +
                    "JOIN transfer_types ON transfer_types.transfer_type_id = transfers.transfer_type_id " +
                    "JOIN accounts ON accounts.account_id = transfers.account_from OR accounts.account_id = transfers.account_to " +
                    "JOIN users ON users.user_id = accounts.user_id " +
                    "WHERE users.user_id = @userID", conn);

                    sqlCommand.Parameters.AddWithValue("@userID", userID);

                SqlDataReader reader = sqlCommand.ExecuteReader(); 
                while(reader.Read())
                {
                    Transfer t = GetTransferFromReader(reader);
                    transferList.Add(t);
                }
            }

            return transferList;
        }
    }
}
