using Siccar.Shallow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
{
    public interface IProgressReportRequestor
    {
        Task<HashSet<ProcessSchema>> FetchProgress(string idToken);
    }
}