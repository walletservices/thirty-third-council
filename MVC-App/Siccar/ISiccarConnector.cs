using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MVC_App
{
    public interface ISiccarConnector
    {
        Task<string> GetStepNextOrStartProcess(string processid, string version, string idToken, string secondaryToken = null);

        Task<string> GetProgressReport(string idToken);

        Task<string> SubmitStep(dynamic content, string idToken, string transactionId);

        Task<string> GetTransaction(string idToken, string transactionId);

        Task<FileContentResult> GetDocumentTransaction(string idToken, string transactionId);
    }
}
