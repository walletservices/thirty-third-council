using Microsoft.Extensions.Caching.Memory;
using siccar_cache.Caches;
using System.Collections.Generic;
using System.Linq;

namespace Siccar.CacheManager.Caches
{
    public class UserCache : AbstractCache<string, string>, IUserCache
    {

        private static string KEY_ID = "user_";
        public UserCache(IMemoryCache cache) : base(cache)
        {
        }

        private string BuildKey(string id)
        {
            return $"{KEY_ID}{id}";
        }

        public List<string> GetLoggedInUsers()
        {
            var allCache = GetAll();  // all potential strings 
            var allKeys = allCache.Where(x => x.StartsWith(KEY_ID));  
            return allKeys.Select(x => x.Substring(KEY_ID.Length)).ToList();
        }

        public string GetIdToken(string key)
        {
            return GetValue(BuildKey(key));
        }

        public void AddUser(string key, string idToken)
        {
            AddValue(BuildKey(key), idToken);
        }

        public void RemoveUser(string key)
        {
            RemoveEntry(BuildKey(key));

        }
        public bool IsUserLoggedIn(string key)
        {
            return KeyExistsInCache(BuildKey(key));
        }

        public string IdToken(string key)
        {
            return GetValue(BuildKey(key));
        }
    }
}
