using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
namespace BMCIT.Services
{
    public interface IAccountService
    {
        Response Signup(Users data);
        Response Login(Login data);
        Response Verify(string token);
        Response ForgetPassword(string token);
        Response ResetPassword(string token);
        IEnumerable<Users> GetAllUsers{ get; }
        IEnumerable<Users> GetAllAdmins{ get; }
    }
}