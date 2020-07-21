using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly APIService APIService = new APIService();
        private static readonly AuthService authService = new AuthService();

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        API_User user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers"); //Sub menu needs to have "Transfer Details"
                Console.WriteLine("3: View your pending requests"); // Sub menu needs to have "Approve or reject pending transfer" where they can be approved/rejected with a "0" for cancel
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    // View your current balance
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"Your current account balance is: ${APIService.GetUserBalance()}");
                    Console.WriteLine("-------------------------------------------");
                }
                else if (menuSelection == 2)
                {
                    List<TransferHistory> historyList = APIService.GetTransferHistory();                    
                    consoleService.HistorcialTransfers(historyList); 
                }
                else if (menuSelection == 3)
                {
                    //ConsoleService.PendingRequests(); // Still need to figure out return type.
                }
        
                else if (menuSelection == 4)
                {
                    // Send TE bucks
                    //Console.WriteLine("-------------------------------------------\n" +
                    //    "Users\nID\t\t\tName\n" +
                    //    "-------------------------------------------\n" +
                    //    /*$*/"{/*User ID*/}\t\t\t{/*User name*/}\n" + // Maybe replace with a list of users? // Shows misalligned until /*User ID*/ is replaced with a 2 or 3 digit interger
                    //    /*$*/"{/*User ID*/}\t\t\t{/*User name*/}\n" + // Maybe replace with a list of users? // Shows misalligned until /*User ID*/ is replaced with a 2 or 3 digit interger
                    //    "---------\n" +
                    //    "\n" +
                    //    "Enter ID of user you are sending to (0 to cancel):\n" +
                    //    "Enter amount:");

                    // Console.ReadLine() find ID to send money to

                    consoleService.PrintUsers(APIService.GetUsers());
                    TransferDetails goodTransfer = APIService.PostTransfer(consoleService.PromptForTransferInfo());
                    consoleService.PrintTransferDetails(goodTransfer);
                    
                }
                else if (menuSelection == 5)
                {
                    // Request TE bucks
                    Console.WriteLine("-------------------------------------------\n" +
                        "Users\n" +
                        "ID\t\t\tName\n" +
                        "-------------------------------------------\n" +
                        /*$*/"{/*User ID*/}\t\t\t{/*User name*/}\n" + // Maybe replace with a list of users?
                        /*$*/"{/*User ID*/}\t\t\t{/*User name*/}\n" + // Maybe replace with a list of users?
                        "---------\n" +
                        "\n" +
                        "Enter ID of user you are requesting from (0 to cancel):\n" +
                        "Enter amount:");
                }
                else if (menuSelection == 6)
                {
                    // Log in as different user
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    Run(); //return to entry point
                }
                else
                {
                    // Exit
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
