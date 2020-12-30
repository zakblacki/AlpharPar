using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using AlphaParWcfServiceLibrary.Models;
using System.ServiceModel;
using System.IdentityModel.Tokens.Jwt;

namespace AlphaParWcfServiceLibrary
{
    class AuthService
    {
        private static DataAccessService db = new DataAccessService();

        public string LoginWithAD(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                db.WriteLog(null, "Login failed");
                throw new FaultException<ServiceFault>(new ServiceFault("Error: Please provide username and password"));
            }
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "alphapar.ga"))
            {
                bool isvalid = pc.ValidateCredentials(username, password);
                UserToken userToken = new UserToken(username);
                if (isvalid)
                {
                    db.WriteLog(userToken.username, "Login success");
                    return userToken.tokenString;
                }
                else
                {
                    db.WriteLog(null, "Login failed");
                    //throw new Exception("Error: Could not log you in");
                    throw new FaultException<ServiceFault>(new ServiceFault("Error: Could not log you in"));
                }
            }
        }

        public string getUsernameByToken(string tokenString)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);
                if ((long)token.Payload.First(payload => payload.Key == "exp").Value < DateTime.Now.Ticks)
                    throw new FaultException<ServiceFault>(new ServiceFault("Error: Token expired"));

                return token.Payload.First(payload => payload.Key == "username").Value.ToString();
            }
            catch(Exception ex)
            {
                //throw new Exception("Error: Token expired or invalid");
                db.WriteLog(null, "getUsernameByToken failed");
                throw new FaultException<ServiceFault>(new ServiceFault("Error: Token expired or invalid"));
            }
        }
    }
}
