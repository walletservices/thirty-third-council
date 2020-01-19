using System.Threading.Tasks;
using MVC_App.Models;

namespace MVC_App.Siccar
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
    }
}