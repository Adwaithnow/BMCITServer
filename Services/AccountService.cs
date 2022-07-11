using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BMCIT.Services
{
    public class AccountService : IAccountService
    {
        public string path = "Databases/Users.json";
        Response res = new Response();
        public IEnumerable<Users> GetAllUsers => JsonConvert.DeserializeObject<List<Users>>(System.IO.File.ReadAllText(path));
        public IEnumerable<Users> GetAllAdmins => JsonConvert.DeserializeObject<List<Users>>(System.IO.File.ReadAllText(path)).Where(x => x.Role == "Admin");
        public Response Signup(Users usersdata)
        {
            List<Users> olddta = GetAllUsers.ToList();
            Response res = new Response();
            if (olddta.Any(u => u.Email == usersdata.Email))
            {
                res.ResCode = 405;
                res.RData = "User Already Exist";
                return res;
            }

            CreatePasswordHash(usersdata.Password,
                   out byte[] passwordHash,
                   out byte[] passwordSalt);
            Users LoginData = new Users
            {
                Id = Guid.NewGuid().ToString(),
                Email = usersdata.Email,
                Password = usersdata.Password,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                FirstName = usersdata.FirstName,
                LastName = usersdata.LastName,
                Role = "User"
            };
            olddta.Add(LoginData);
            bool userop = JsonOp.WriteData(olddta, "Databases/Users.json");
            if (!userop)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured";
                return res;
            }
            else
            {
                res.ResCode = 201;
                res.RData = "User Registered Successfully";
            }
            return res;
        }
        public Response Login(UserLogin userdata)
        {
            Response res = new Response();
            Users user = GetAllUsers.FirstOrDefault(u => u.Email == userdata.Email);
            if (user == null)
            {
                res.ResCode = 405;
                res.RData = "No User Found";
                return res;
            }
            if (!VerifyPasswordHash(userdata.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
            {
                res.ResCode = 405;
                res.RData = "Password is incorrect.";
                return res;
            }
            res.RData = CreateToken(user);
            res.ResCode = 200;
            return res;
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
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Users user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my top secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}