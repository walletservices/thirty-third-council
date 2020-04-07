using Siccar.CacheManager.Models;

namespace Siccar.CacheManager.ModelManagers
{
    public interface ISiccarStatusCacheResponseManager
    {
        SiccarStatusCacheResponse BuildSiccarStatusCacheResponseForUser(string userId);
    }
}