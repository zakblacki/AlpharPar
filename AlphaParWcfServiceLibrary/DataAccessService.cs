using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using AlphaParWcfServiceLibrary.Models;

namespace AlphaParWcfServiceLibrary
{
    class DataAccessService
    {
        MongoClient client;
        IMongoDatabase db;

        public DataAccessService()
        {
            client = new MongoClient(
               new MongoClientSettings
               {
                   Server = new MongoServerAddress("localhost", 27017),
                   ServerSelectionTimeout = TimeSpan.FromSeconds(3),
                   Credential = MongoCredential.CreateCredential("admin", "MongoDBAdmin", "yeH7DG3d")
               }
            );
            db = client.GetDatabase("alphapar");
        }

        public void WriteLog(string username, string action)
        {
            IMongoCollection<Log> collection = db.GetCollection<Log>("logs");
            Log log = new Log(username, action);
            collection.InsertOne(log);
        }

        public List<Client> GetClients()
        {
            IMongoCollection<Client> collection = db.GetCollection<Client>("clients");
            List<Client> clients = collection.Find(_ => true).ToList();
            return clients;
        }

        public void InsertClient(Client client)
        {
            IMongoCollection<Client> collection = db.GetCollection<Client>("clients");
            collection.InsertOne(client);
        }

        public void DeleteClient(Client client)
        {
            IMongoCollection<Client> collection = db.GetCollection<Client>("clients");
            collection.DeleteOne(a => a.ID == client.ID);
        }
    }
}
