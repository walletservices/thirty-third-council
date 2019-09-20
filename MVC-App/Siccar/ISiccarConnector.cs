using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public interface ISiccarConnector
    {
        Task<string> GetStepNextOrStartProcess(string processid, string version, string idToken);

        Task<string> GetProgressReport(string idToken);

        Task<string> SubmitStep(dynamic content, string idToken, string transactionId);

    }
}
