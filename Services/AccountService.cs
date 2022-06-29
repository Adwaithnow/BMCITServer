using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class AccountService : IAccountService
    {
        public string path = "Databases/Users.json";
        Response res = new Response();
        public IEnumerable<Users> GetAllUsers => JsonConvert.DeserializeObject<List<Users>>(System.IO.File.ReadAllText(path));
        public IEnumerable<Users> GetAllAdmins => JsonConvert.DeserializeObject<List<Users>>(System.IO.File.ReadAllText(path)).Where(x => x.IsAdmin == true);
        public Response Signup(Users data)
        {
            IEnumerable<Users> olddta = GetAllUsers.Append(data);
            return WriteUser(olddta);
        }
        public Response Login(Login data)
        {
            throw new NotImplementedException();
        }
        public Response Verify(string token)
        {
            throw new NotImplementedException();
        }
        public Response ForgetPassword(string token)
        {
            throw new NotImplementedException();
        }
        public Response ResetPassword(string token)
        {
            throw new NotImplementedException();
        }
        public Response WriteUser(IEnumerable<Users> userdata)
        {
            try
            {
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(userdata, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "You are successfully registered now you can login !";
            return res;
        }
    }
}