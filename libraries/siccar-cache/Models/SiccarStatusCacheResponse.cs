using Siccar.Shallow.Models;
using System.Collections.Generic;

namespace Siccar.CacheManager.Models
{
    public class SiccarStatusCacheResponse
    {
        public HashSet<SiccarStatusCacheProcessResponse> ProcessResponses { get; set; }
        public HashSet<ProgressReport> ProgressReports { get; set; }

        public SiccarStatusCacheResponse()
        {
            ProcessResponses = new HashSet<SiccarStatusCacheProcessResponse>();
            ProgressReports = new HashSet<ProgressReport>();
        }
    }
}
