using MVC_App.Models;

namespace MVC_App.Cache
{
    public interface ISiccarStatusCacheResponseManager
    {
        SiccarStatusCacheResponse BuildSiccarStatusCacheResponseForUser(string userId);
    }
}