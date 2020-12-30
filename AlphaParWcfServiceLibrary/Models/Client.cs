using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlphaParWcfServiceLibrary.Models
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("firstname")]
        public string firstname { get; set; }
        [BsonElement("lastname")]
        public string lastname { get; set; }
        [BsonElement("address")]
        public string address { get; set; }
        [BsonElement("phone")]
        public string phone { get; set; }
    }
}
