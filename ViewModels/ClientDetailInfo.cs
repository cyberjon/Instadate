using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.ViewModels
{
    public class ClientDetailInfo
    {
        public Client client { get; set; }
        public List<string> interests { get; set; }

        public ClientDetailInfo(Client client, List<string> interests)
        {
            this.client = client;
            this.interests = interests;
        }
    }

   
   
}