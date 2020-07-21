using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Services;

namespace TenmoServer.Models
{
    [Route("[controller]")]
    [ApiController]
    [Authorize] 
    public class TransferController : Controller
    {
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        private readonly ITransferDAO transferDAO;
                
        public TransferController(IAccountDAO _accountDAO, IUserDAO _userDAO, ITransferDAO _transferDAO)
        {
            accountDAO = _accountDAO;
            userDAO = _userDAO;
            transferDAO = _transferDAO;
        }

        

        //      https://localhost:44315/tranfer
        [HttpPost("send")]
        public IActionResult PostTransfer(Transfer transfer)
        {           

            TransferService service = new TransferService(accountDAO, userDAO, transferDAO);

            string confirmationMessage = service.ProcessTheTransfer(transfer); //successful processing returns a new transfer ID number.

            IActionResult result = BadRequest(new { message = confirmationMessage });

            if (int.TryParse(confirmationMessage, out int transID))
            {
                transfer.ID = transID;
                TransferDetails completedTransfer = service.GetTransferDetails(0,transfer); 
                result = Ok(completedTransfer);
            }

            return result;
        }

        [HttpGet("details/{id}")]
        public IActionResult GetTransactionDetails(int id)
        {
           
            TransferService service = new TransferService(accountDAO, userDAO, transferDAO);
            IActionResult result = BadRequest(new { message = "Transfer details could not be found." });
            TransferDetails transferDetails = service.GetTransferDetails(id);
            if (transferDetails != null)
            {
                result = Ok(transferDetails);
            }

            return result;
        }

        [HttpGet("users")]
        public ActionResult<List<PublicUser>> GetListOfUsers()
        {
            TransferService service = new TransferService(accountDAO,userDAO,transferDAO);
            List<PublicUser> publicUsers = service.GetUsers();
            return Ok(publicUsers);
        }

        [HttpGet("list")]
        public IActionResult GetTransferList()
        {
            var userID = Convert.ToInt32(User.FindFirst("sub")?.Value);
            //int id = Convert.ToInt32(userID);

            TransferService service = new TransferService(accountDAO, userDAO, transferDAO);
            IActionResult result = BadRequest(new { message = "Transfer list could not be found." });
            List<TransferHistory> historyList = service.GetTransferHistory(userID);

            if (historyList != null)
            {
                result = Ok(historyList);
            }

            return result;
        }

        //[HttpGet("Test")]
        //public int TestMethod()
        //{
        //    int pickANumber = 4;
        //    return pickANumber;
        //}

    }
}