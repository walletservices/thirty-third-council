using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public interface ISiccarOptions
    {
        string addUserToGroup { get; set; }
        string createActors { get; set; }
        string getGroups { get; set; }
        string getUsers { get; set; }
        string processDesignersId { get; set; }
        string getUsersWallets { get; set; }
        string removeUserFromGroup { get; set; }
    }
}
