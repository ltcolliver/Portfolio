using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    class APIService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
         

        public decimal GetUserBalance()
        {
            decimal userBalance = 0.0M;
            string token = UserService.GetToken();
            RestRequest request = new RestRequest(API_BASE_URL + "account/balance");
            client.Authenticator = new JwtAuthenticator(token); 
            
            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                userBalance = response.Data;
            }
            return userBalance;
        }

        public List<UserData> GetUsers()
        {
            string token = UserService.GetToken();
            RestRequest request = new RestRequest(API_BASE_URL + "Transfer/users");
            client.Authenticator = new JwtAuthenticator(token);
            
            IRestResponse<List<UserData>> response = client.Get<List<UserData>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

        public TransferDetails PostTransfer(API_Transfer transfer)
        {
            string token = UserService.GetToken();
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/send");
            client.Authenticator = new JwtAuthenticator(token);
            request.AddJsonBody(transfer);
             

            IRestResponse<TransferDetails> response = client.Post<TransferDetails>(request); // response to be informative if the transfer error was due to bad input from user.

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

        public TransferDetails GetTransferDetails(int transID)
        {
            //TransferDetails details = new TransferDetails();

           
            string token = UserService.GetToken();
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/details/{transID}");
            client.Authenticator = new JwtAuthenticator(token);

            IRestResponse<TransferDetails> response = client.Get<TransferDetails>(request); // response to be informative if the transfer error was due to bad input from user.

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

            return null;
        }
         public List<TransferHistory> GetTransferHistory()
        {
           
            string token = UserService.GetToken();
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/list");
            client.Authenticator = new JwtAuthenticator(token);

            IRestResponse<List<TransferHistory>> response = client.Get<List<TransferHistory>>(request); // response to be informative if the transfer error was due to bad input from user.

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

            return null;
        }
    }
}


//transfer/list