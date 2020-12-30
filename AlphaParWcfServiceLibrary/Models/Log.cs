using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AlphaParWcfServiceLibrary.Models
{
    class Log
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("username")]
        string username { get; set; }
        [BsonElement("action")]
        string action { get; set; }
        [BsonElement("dateTime")]
        DateTime dateTime { get; set; }

        public Log(string username, string action)
        {
            this.username = username;
            this.action = action;
            dateTime = DateTime.UtcNow.AddHours(1);
        }
    }
}
