using Siccar.Shallow.Models;
using System.Collections.Generic;

namespace Siccar.CacheManager.Caches
{
    public interface IProcessSchemaCache
    {
        void AddSchema(string userId, ProcessSchema schema);
        void AddSchemas(string userId, HashSet<ProcessSchema> schemas);
        bool ContainsAll(string userId, HashSet<ProcessSchema> schemas);
        HashSet<ProcessSchema> GetSchemas(string userId);
    }
}