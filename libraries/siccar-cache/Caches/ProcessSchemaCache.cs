using Microsoft.Extensions.Caching.Memory;
using Siccar.Shallow.Models;
using siccar_cache.Caches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Siccar.CacheManager.Caches
{
    public class ProcessSchemaCache : AbstractCollectionCache<ProcessSchema>, IProcessSchemaCache
    {
        private static string KEY_ID = "ps_";

        public ProcessSchemaCache(IMemoryCache cache) : base(cache)
        {
        }

        private string BuildKey(string id)
        {
            return $"{KEY_ID}{id}";
        }

        public new bool ContainsAll(string key, HashSet<ProcessSchema> values)
        {
            return base.ContainsAll(BuildKey(key), values);
        }


        public void AddSchema(string userId, ProcessSchema schema)
        {
            base.AddValue(BuildKey(userId), schema);
        }

        public void AddSchemas(string userId, HashSet<ProcessSchema> schemas)
        {
            base.AddValues(BuildKey(userId), schemas);
        }

        public HashSet<ProcessSchema> GetSchemas(string userId)
        {
            return GetValues(BuildKey(userId));
        }
    }
}
