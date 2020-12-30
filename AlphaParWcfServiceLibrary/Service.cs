using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.DirectoryServices.AccountManagement;
using AlphaParWcfServiceLibrary.Models;
using System.ServiceModel.Web;

namespace AlphaParWcfServiceLibrary
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Service : IService
    {
        AuthService authService = new AuthService();
        DataAccessService dataAccessService = new DataAccessService();

        public string LoginWithAD(string username, string password)
        {
            return authService.LoginWithAD(username, password);
        }

        public string GetUsernameByToken(string token)
        {
            return authService.getUsernameByToken(token);
        }

        public List<Client> GetClients(string token)
        {
            try
            {
                string username = authService.getUsernameByToken(token);

                dataAccessService.WriteLog(username, "GetClients");
                List<Client> clients = dataAccessService.GetClients();
                return clients;
            }
            catch (Exception ex)
            {
                dataAccessService.WriteLog(null, "GetClients failed");
                throw ex;
            }
        }

        public Client InsertClient(string token, Client client)
        {
            if (string.IsNullOrEmpty(client.address) || 
                string.IsNullOrEmpty(client.firstname) || 
                string.IsNullOrEmpty(client.lastname) || 
                string.IsNullOrEmpty(client.phone)
                )
                throw new FaultException<ServiceFault>(new ServiceFault("Error: Please provide all required information for the new client."));
            try
            {
                string username = authService.getUsernameByToken(token);

                dataAccessService.WriteLog(username, "InsertClient");
                dataAccessService.InsertClient(client);
                return client;
            }
            catch (Exception ex)
            {
                dataAccessService.WriteLog(null, "InsertClient failed");
                throw ex;
            }
        }

        public bool DeleteClient(string token, Client client)
        {
            try
            {
                string username = authService.getUsernameByToken(token);

                dataAccessService.WriteLog(username, "DeleteClient");
                dataAccessService.DeleteClient(client);
                return true;
            }
            catch (Exception ex)
            {
                dataAccessService.WriteLog(null, "DeleteClient failed. ClientID="+client.ID);
                throw ex;
            }
        }
    }
}
