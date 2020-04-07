using System.Collections.Generic;

namespace Siccar.CacheManager.Caches
{
    public interface IUserCache
    {
        void AddUser(string guid, string idToken);
        string GetIdToken(string key);
        List<string> GetLoggedInUsers();
        bool IsUserLoggedIn(string guid);
        void RemoveUser(string guid);
    }
}