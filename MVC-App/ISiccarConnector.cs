using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public interface ISiccarConnector
    {
        ISiccarOptions Options { get; set; }
        List<KeyValuePair<string, string>> getWallets();
        List<KeyValuePair<string, string>> getWalletsUserIsIn(string userId);
        List<KeyValuePair<string, string>> getWalletsUserIsNotIn(string userId);
        bool isUserInProcessDesignersGroup(string userId);
        void removeUserFromGroup(string userId, string walletId);
        void addUserToGroup(string userId, string processDesignersGroup);
        List<KeyValuePair<string, string>> getUsers(string startingLetter);
        void createActor(string userId, string groupId);
        void setToken(string token);
        
    }
}
