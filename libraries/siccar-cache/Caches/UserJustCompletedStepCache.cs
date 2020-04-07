using Microsoft.Extensions.Caching.Memory;
using siccar_cache.Caches;
using System.Collections.Generic;

namespace Siccar.CacheManager.Caches
{
    public class UserJustCompletedStepCache : AbstractCache<string,string>, IUserJustCompletedStepCache
    {
        private static string KEY_ID = "just_comp_";
        
        public UserJustCompletedStepCache(IMemoryCache cache) : base(cache)
        {
        }

        private string BuildKey(string id)
        {
            return $"{KEY_ID}{id}";
        }

        public void AddUser(string key, string value)
        {
            AddValue(BuildKey(key), value);
        }

        public void RemoveUser(string key)
        {
            RemoveEntry(BuildKey(key));
        }

        public bool ContainsUser(string key)
        {
            return KeyExistsInCache(BuildKey(key));
        }

        public string GetSchema(string key)
        {
            return GetValue(BuildKey(key));
        }

        public bool UserAndSchemaStored(string key, string value)
        {
            return GetValue(BuildKey(key)) == value;
        }
    }
}
