using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService AccountService =null;
        public AccountsController(IAccountService accountService)
        {
            AccountService = accountService;            
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Account Is Workings");
        }
        [HttpPost("Signup")]
        public IActionResult Signup(Users user)
        {
            user.Id=Guid.NewGuid().ToString();
            Response res=AccountService.Signup(user);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userdata)
        {
            Response res = AccountService.Login(userdata);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(AccountService.GetAllUsers);
        }
        [HttpGet("GetAllAdmins")]
        public IActionResult GetAllAdmins()
        {
            return Ok(AccountService.GetAllAdmins);
        }
    }
}
