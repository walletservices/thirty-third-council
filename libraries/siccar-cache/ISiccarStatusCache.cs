using Siccar.CacheManager.Models;
using System.Threading.Tasks;

namespace Siccar.CacheManager
{
    public interface ISiccarStatusCache
    {
        void AddUserToJustCompletedStepCache(string userId, string schemaId);
        SiccarStatusCacheResponse GetStatus(string userId);
        bool HasUserBeenProcessed(string userId);
        Task RefreshAndDontWait(string guid, string idToken);
        void RemoveUser(string userId);
        void RemoveUserFromJustCompletedStepCache(string userId);
        Task UpdateStatusInCacheForEveryUser();
        void UpdateProgressReportToReflectSubmission(string guid);
        void ReflectSubmissionFromUser(string userId, string schemaId);
    }
}