using System.Collections.Generic;

namespace MVC_App.Cache.Caches
{
    public interface IProcessSchemaCache
    {
        void AddSchema(string userId, ProcessModel.ProcessSchema schema);
        void AddSchemas(string userId, HashSet<ProcessModel.ProcessSchema> schemas);
        bool ContainsAll(string userId, HashSet<ProcessModel.ProcessSchema> schemas);
        HashSet<ProcessModel.ProcessSchema> GetSchemas(string userId);
    }
}