namespace Siccar.CacheManager.Caches
{
    public interface IUserJustCompletedStepCache
    {
        void AddUser(string userId, string schema);
        void RemoveUser(string userId);
        bool UserAndSchemaStored(string userId, string schemaId);
        bool ContainsUser(string userId);
        string GetSchema(string userId);
    }
}