using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_App.Cache.Requestors
{
    public interface IProgressReportRequestor
    {
        Task<HashSet<ProcessModel.ProcessSchema>> FetchProgress(string idToken);
    }
}