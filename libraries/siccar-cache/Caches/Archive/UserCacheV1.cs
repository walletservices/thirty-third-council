//using System.Collections.Generic;
//using System.Linq;

//namespace Siccar.CacheManager.Caches
//{
//    public class UserCacheV1 : IUserCache
//    {
//        public Dictionary<string, string> _usersLoggedIn;

//        public UserCacheV1()
//        {
//            _usersLoggedIn = new Dictionary<string, string>();
//        }

//        public List<string> GetLoggedInUsers()
//        {
//            return _usersLoggedIn.Keys.ToList<string>();
//        }

//        public string GetIdToken(string key)
//        {
//            return _usersLoggedIn[key];
//        }

//        public void AddUser(string guid, string idToken)
//        {

//            if (IsUserLoggedIn(guid))
//            {
//                _usersLoggedIn[guid] = idToken;
//            }
//            else
//            {
//                _usersLoggedIn.Add(guid, idToken);
//            }

//        }

//        public void RemoveUser(string guid)
//        {
//            if (IsUserLoggedIn(guid))
//            {
//                _usersLoggedIn.Remove(guid);
//            }

//        }
//        public bool IsUserLoggedIn(string guid)
//        {
//            return _usersLoggedIn.ContainsKey(guid);
//        }

//        public string IdToken(string guid)
//        {
//            return _usersLoggedIn[guid];
//        }
//    }
//}
