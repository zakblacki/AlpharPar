using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace AlphaParWebApplication.Models
{
    public class ClientWrapper : ServiceReference1.Client
    {
        public string urlEncodedJsonClient { get; set; }

        public ClientWrapper(ServiceReference1.Client client)
        {
            this.ID = client.ID;
            this.lastname = client.lastname;
            this.firstname = client.firstname;
            this.address = client.address;
            this.phone = client.phone;
            var json = new JavaScriptSerializer().Serialize(client);
            json = Regex.Replace(json, "\\\"ExtensionData\\\":{},", String.Empty);
            this.urlEncodedJsonClient = HttpUtility.UrlEncode(json);
        } 
    }
}