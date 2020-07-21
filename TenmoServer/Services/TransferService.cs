using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Services
{
    public class TransferService
    {
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        private readonly ITransferDAO transferDAO;
        //private readonly int authID;
        
                
        public TransferService(IAccountDAO _accountDAO, IUserDAO _userDAO, ITransferDAO _transferDAO)
        {
            accountDAO = _accountDAO;
            userDAO = _userDAO;
            transferDAO = _transferDAO;
            //authID = GetUserID();
        }

        public string ProcessTheTransfer(Transfer transfer)
        {
            // should add a transaction around the sql calls
            string confirmationMessage = "Recipient Not Found";
            decimal fromBalance = accountDAO.GetBalance(transfer.FromUserID);
            decimal toBalance = accountDAO.GetBalance(transfer.ToUserID);
            //decimal amountToTransfer = transfer.Amount;

            // check for valid recipient ID#

            List<User> validUsers = userDAO.GetUsers();

            foreach (User thisUser in validUsers) 
            {
                if (thisUser.UserId == transfer.ToUserID)
                {                 
                    if(accountDAO.GetBalance(transfer.FromUserID) >= transfer.Amount)
                    {
                        fromBalance -= transfer.Amount;
                        toBalance += transfer.Amount;
                        
                        accountDAO.UpdateBalance(transfer.FromUserID, fromBalance);
                        accountDAO.UpdateBalance(transfer.ToUserID, toBalance);
                        confirmationMessage = Convert.ToString(transferDAO.MakeTransfer(transfer));
                    }
                    else
                    {
                        confirmationMessage = "Insufficient Funds";
                    }
                }
                //else
                //{
                //    confirmationMessage = "Recipient Not Found";
                //}
            }


            // Lee is pissed cause we're aren't user friendly "enough" 
            // Actually, it's ok. This isn't a real program. 
            
            return confirmationMessage;
        }

        //public List<PublicUser> GetUsers()
        //{
        //    List<PublicUser> allUsers_publicInfo = new List<PublicUser>();
        //    List<User> allUsers_rawData = userDAO.GetUsers();

        //    foreach (User user in allUsers_rawData)
        //    {
        //        PublicUser publicUser = new PublicUser();                
        //        publicUser.UserId = user.UserId;
        //        publicUser.Username = user.Username;
                
        //        allUsers_publicInfo.Add(publicUser);
                
        //    }
        //    return allUsers_publicInfo;
        //}

        public List<PublicUser> GetUsers()
        {
            List<PublicUser> allUsers_publicInfo = new List<PublicUser>();
            List<User> allUsers_rawData = userDAO.GetUsers();

            foreach (User user in allUsers_rawData)
            {
                PublicUser publicUser = new PublicUser();
               
                    publicUser.UserId = user.UserId;
                    publicUser.Username = user.Username;
                    allUsers_publicInfo.Add(publicUser);
                
            }

            return allUsers_publicInfo;
        }

        public string GetUserName(int userID)
        {
            List<User> allUsers_rawData = userDAO.GetUsers();
            string username = "";

            foreach (User thisUser in allUsers_rawData)
            {
                if (thisUser.UserId == userID)
                {
                    username = thisUser.Username;
                }
            }

            return username;
        }

        public TransferDetails GetTransferDetails(int transID = 0, Transfer transfer = null) // this will allow a parameterless method call
        {
            TransferDetails details = new TransferDetails();
            
            if (transID != 0)
            {                
                transfer = transferDAO.GetTransferDetails(transID);
            }
            
            string senderName = GetUserName(transfer.FromUserID);
            string receiverName = GetUserName(transfer.ToUserID);
            details.ID = transfer.ID;
            details.FromUser = senderName;
            details.ToUser = receiverName;
            details.Type = transfer.Type;
            details.Status = transfer.Status;
            details.Amount = transfer.Amount;

            
            return details;
        }

        public List<TransferHistory> GetTransferHistory(int userID)
        {
            List<TransferHistory> historyList = new List<TransferHistory>();
            
            List<Transfer> tranferData = transferDAO.TransferHistory(userID);

            foreach (Transfer rawInfo in tranferData)
            {
                TransferHistory history = new TransferHistory();
                history.Amount = rawInfo.Amount;
                history.TransferID = rawInfo.ID;
                if (userID == rawInfo.FromUserID)
                {
                    history.Username = GetUserName(rawInfo.ToUserID);
                    history.Direction = "To";
                }
                else
                {
                    history.Username = GetUserName(rawInfo.ToUserID);
                    history.Direction = "From"; 
                }
                
                historyList.Add(history);
            }
            return historyList;  
        }

    }
}
// public int TransferID { get; set; }
//public string Username { get; set; }
//public string Direction { get; set; } // ("From" "To")
//public decimal Amount { get; set; }