using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            return StatusCode(res.ResCode,res);
        }
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userdata)
        {
            // Response rd=new Response();
            // Console.WriteLine(userdata.Email);
            // Console.WriteLine(userdata.Password);

            Response res = AccountService.Login(userdata);
            // Console.WriteLine(res.ResCode);
            // Console.WriteLine(res.RData);
        Console.WriteLine( JsonConvert.SerializeObject(res));
            return StatusCode(res.ResCode, res);
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
