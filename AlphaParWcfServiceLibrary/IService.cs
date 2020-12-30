using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AlphaParWcfServiceLibrary.Models;

namespace AlphaParWcfServiceLibrary
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string LoginWithAD(string username, string password);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetUsernameByToken(string token);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<Client> GetClients(string token);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        Client InsertClient(string token, Client client);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteClient(string token, Client client);
    }
}
