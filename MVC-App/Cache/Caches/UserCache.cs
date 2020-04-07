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
            
            if (IsUserLoggedIn(guid))
            {
                Console.WriteLine("Updatinguser" + guid);
                _usersLoggedIn[guid] = idToken;
            }
            else
            {
                Console.WriteLine("Adding new user" + guid);
                _usersLoggedIn.Add(guid, idToken);
            }

        }

        public void RemoveUser(string guid)
        {
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
