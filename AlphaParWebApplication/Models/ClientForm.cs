using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaParWebApplication.Models
{
    public class ClientForm
    {
        public ServiceReference1.Client client { get; set; }

        public ClientForm()
        {
            client = new ServiceReference1.Client();
        }
    }
}