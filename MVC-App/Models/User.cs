using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Models
{
    public class User
    {
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }

        public int age { get; set; }
        public string country { get; set; }
    }
}
