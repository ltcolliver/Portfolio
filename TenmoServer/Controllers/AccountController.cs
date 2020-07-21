using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        //private readonly ITokenGenerator tokenGenerator;
        //private readonly IPasswordHasher passwordHasher;
        private readonly IUserDAO userDAO;
        private readonly IAccountDAO accountDAO;

        public AccountController(IAccountDAO _accountDAO, IUserDAO _userDAO)
        {
            //tokenGenerator = _tokenGenerator;
            //passwordHasher = _passwordHasher;
            userDAO = _userDAO;
            // accountDAO = new AccountSqlDAO();
            accountDAO = _accountDAO;
        }

        [HttpGet("balance")] // Needs to use token & Authentication
        public IActionResult GetBalance()
        {// Pull the ID out of the token.
            //int userIdentity = userDAO.GetUser
            var userID = User.FindFirst("sub")?.Value;
            int id = Convert.ToInt32(userID);
            return Ok(accountDAO.GetBalance(id));
        } 
      
    }
}
