using System.Collections.Generic;

namespace MVC_App.Cache.Caches
{
    public interface IUserCache
    {
        void AddUser(string guid, string idToken);
        string GetIdToken(string key);
        List<string> GetLoggedInUsers();
        string IdToken(string guid);
        bool IsUserLoggedIn(string guid);
        void RemoveUser(string guid);
    }
}