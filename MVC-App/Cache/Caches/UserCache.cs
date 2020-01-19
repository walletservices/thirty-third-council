using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Cache.Caches
{
    public class UserCache : IUserCache
    {
        public Dictionary<string, string> _usersLoggedIn;

        public UserCache()
        {
            _usersLoggedIn = new Dictionary<string, string>();
        }

        public List<string> GetLoggedInUsers()
        {
            return _usersLoggedIn.Keys.ToList<string>();
        }

        public string GetIdToken(string key)
        {
            return _usersLoggedIn[key];
        }

        public void AddUser(string guid, string idToken)
        {
            Console.WriteLine($"Adding user {guid} to cache ");

            if (IsUserLoggedIn(guid))
            {
                _usersLoggedIn[guid] = idToken;
            }
            else
            {
                _usersLoggedIn.Add(guid, idToken);
            }

        }

        public void RemoveUser(string guid)
        {
            Console.WriteLine($"User {guid} removed from cache");
            if (IsUserLoggedIn(guid))
            {
                Console.WriteLine("removing user" + guid);
                _usersLoggedIn.Remove(guid);
            }

        }
        public bool IsUserLoggedIn (string guid)
        {
            return _usersLoggedIn.ContainsKey(guid);
        }

        public string IdToken(string guid)
        {
            return _usersLoggedIn[guid];
        }
    }
}
