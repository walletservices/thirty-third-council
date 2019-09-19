using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public class SiccarOptions : ISiccarOptions
    {
        public string createActors { get; set; }
        public string getUsers { get; set; }
        public string getGroups { get; set; }
        public string addUserToGroup { get; set; }
        public string processDesignersId { get; set; }
        public string getUsersWallets { get; set; }
        public string removeUserFromGroup { get; set; }
    }
}
