using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestSharpAPITests
{
    public class Contact
    {
        public int id { get; set; }

        //[JsonPropertyName("first name")]
        public string firstName { get; set; }

        //[JsonPropertyName("last name")]
        public string lastName { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public DateTime dateCreated { get; set; }

        public string comments { get; set; }
    }
}
