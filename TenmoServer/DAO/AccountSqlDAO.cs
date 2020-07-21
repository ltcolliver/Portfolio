using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        //Temp made a empty construstor
        public AccountSqlDAO()
        {
        }

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public decimal GetBalance(int userID)
        {
            decimal balance = 0.0M;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @userID", conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    balance = Convert.ToDecimal(reader["balance"]); // May need to return entire object?
                }
            }

            return balance;
        }

        public bool UpdateBalance(int userID, decimal newBalance)
        {
            bool isSuccessful = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = @newBalance WHERE user_id = @userID", conn);
                cmd.Parameters.AddWithValue("@newBalance", newBalance);
                cmd.Parameters.AddWithValue("@userID", userID);
                
                if (cmd.ExecuteNonQuery() == 1)
                {
                    isSuccessful = true;
                }
                                
            }


            return isSuccessful;
        }
    }
}
