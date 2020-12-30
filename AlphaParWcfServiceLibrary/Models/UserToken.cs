using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel;
using System.Security;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AlphaParWcfServiceLibrary.Models
{
    class UserToken
    {
        private static Random random = new Random();

        public string username { get; set; }
        public string tokenString { get; set; }

        string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b372742" + 
           "9090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        public UserToken(string username)
        {
            var securityKey = new Microsoft
               .IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                             (securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtHeader(credentials);
            var payload = new JwtPayload
            {
               { "username", username},
               { "iat", DateTime.Now.Ticks },
               { "exp", DateTime.Now.AddHours(1).Ticks }
            };

            var secToken = new JwtSecurityToken(token, payload);
            var handler = new JwtSecurityTokenHandler();

            this.username = username;
            tokenString = handler.WriteToken(secToken);
        }

        //private string GenerateToken()
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    return new string(Enumerable.Repeat(chars, 36).Select(s => s[random.Next(s.Length)]).ToArray());
        //}
    }
}
