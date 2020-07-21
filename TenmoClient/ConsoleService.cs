using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    public class ConsoleService
    {
        List<int> validID = new List<int>(); 
        public void PrintUsers(List<UserData> userList)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID\t\t\tName");
            Console.WriteLine("--------------------------------------------");
            foreach (UserData user in userList)
            {
                Console.WriteLine($"{user.UserId}\t\t\t{user.Username}");
                validID.Add(user.UserId); 
            }
            Console.WriteLine("--------------------------------------------"); 
            

        }





        public API_Transfer PromptForTransferInfo()
        {
            API_Transfer newTransfer = new API_Transfer()
            {
                FromUserID = UserService.GetUserId()
            };
            
  
            Console.WriteLine("Enter ID of user you are sending to (0 to cancel): ");
            string receiverID = Console.ReadLine();
            int ID = -1;
            bool isValid = validID.Contains(ID);
            while (!int.TryParse(receiverID, out ID) && !isValid) 
            {
                if (ID == 0)
                {
                    break;
                }
                Console.WriteLine("Incorrect value submitted. Please enter a valid ID:");
                Console.WriteLine("Enter ID of user you are sending to (0 to cancel): ");
                receiverID = Console.ReadLine();
            }

            // create if statement to check for valid user ID

            newTransfer.ToUserID = ID; 
            
            

            Console.WriteLine("Enter amount:");
            string inputAmount = Console.ReadLine();
            decimal validAmount = 0;
            while (!decimal.TryParse(inputAmount, out validAmount) && validAmount > 0)
            {
                Console.WriteLine("Incorrect value submitted. Please enter a valid amount:");
                Console.WriteLine("Enter valid amount you want to send: ");
                receiverID = Console.ReadLine();
            }


            // create if statement to check for sufficient funds

            newTransfer.Amount = validAmount;

            return newTransfer;
        }

        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int transferID))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return transferID;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }
        
        
        public void HistorcialTransfers(List<TransferHistory> transfers) // Used for menu option #2 // Still need to figure out return type.
        {
            // Do I need to check premission for spefic user's right to access transfers below?

            List<int> validID = new List<int>();
            // View your past transfers
            Console.WriteLine("-------------------------------------------\n" +
                "Transfers\nID\t\t\tFrom/To\t\t\tAmount\n" +
                "-------------------------------------------\n");
            foreach (TransferHistory transfer in transfers)
            {
                Console.WriteLine($"{transfer.TransferID}        {transfer.Direction}: {transfer.Username}      $ {transfer.Amount}");
                validID.Add(transfer.TransferID);
            }
            Console.WriteLine("---------\n" +
                "Please enter transfer ID to view details (0 to cancel): ");
                    // TransferHistory has TransferID, Username, Direction, & Amount
            //Transfer Details
            int subMenuSelection = -1;
            while (subMenuSelection != 0)
            {
                if (!int.TryParse(Console.ReadLine(), out subMenuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (validID.Contains(subMenuSelection))
                {
                    APIService service = new APIService();
                    TransferDetails details = service.GetTransferDetails(subMenuSelection);
                    PrintTransferDetails(details);


                    //Show Transfer details
                    //Console.WriteLine("--------------------------------------------\n" +
                    //    "Transfer Details\n" +
                    //    "--------------------------------------------\n" +
                    //    $"Id: {details.ID}\n" +
                    //    $"From: {details.FromUser}\n" +
                    //    $"To: {details.ToUser}\n" +
                    //    $"Type: {details.Type}\n" + // Send or Request
                    //    $"Status: {details.Status}\n" + // Pending, Approved, Rejected
                    //    $"Amount: ${details.Amount}");
                }
                else if (subMenuSelection == 0)
                {
                    return;
                }
            }
        }

        public void PrintTransferDetails(TransferDetails details)
        {
            Console.WriteLine("--------------------------------------------\n" +
                        "Transfer Details\n" +
                        "--------------------------------------------\n" +
                        $"Id: {details.ID}\n" +
                        $"From: {details.FromUser}\n" +
                        $"To: {details.ToUser}\n" +
                        $"Type: {details.Type}\n" + // Send or Request
                        $"Status: {details.Status}\n" + // Pending, Approved, Rejected
                        $"Amount: ${details.Amount}");
        }

        public void PendingRequests() // Used for menu option #3 // Still need to figure out return type.
        {
            // View your pending requests
            Console.WriteLine("WARNING INCOMPLETE\n" +
                "-------------------------------------------\n" +
                            "Pending Transfers\n" +
                            "ID\t\tTo\t\tAmount\n" +
                            "-------------------------------------------\n" +
                            /*$*/"{/*User ID*/}\t\t{/*User name*/}\t\t$ {/*Amount*/}\n" +
                            /*$*/"{/*User ID*/}\t\t{/*User name*/}\t\t$ {/*Amount*/}\n" +
                            "---------\n" +
                            "Please enter transfer ID to approve/reject (0 to cancel): ");

            // Approve or reject pending transfer
            int subMenuSelection = -1;
            while (subMenuSelection != 0)
            {
                if (!int.TryParse(Console.ReadLine(), out subMenuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (subMenuSelection == /*Valid Transfer ID*/ 1 /*using 1 only as a temporay placeholder*/)
                {
                    Console.WriteLine("1: Approve\n" +
                        "2: Reject\n" +
                        "0: Don't approve or reject\n" +
                        "---------\n" +
                        "Please choose an option:");
                    // if (!int.TryParse(Console.ReadLine(), out wasApproved))
                    // {
                    // Console.WriteLine("Invalid input. Please enter only a number.");
                    // }
                    // else if (wasApproved == 1) // Approve
                    // {
                    // Make the approval
                    // }
                    // else if (wasApproved == 2) // Reject
                    // {
                    // Reject the transfer
                    // }
                    // else if (wasApproved == 0) // Don't approve or reject // Maybe just else?
                    // {
                    // "Don't approve or reject" // Cancel and go back a menu
                    // }
                }
            }
        }
    }
}