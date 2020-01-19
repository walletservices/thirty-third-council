
using MVC_App.Cache;
using System.Collections.Generic;
using System.Linq;

namespace MVC_App.Models
{
    public class SiccarStatusCacheResponse
    {
        public string DISABLEDBLUEBADGEPROCESS = "12374e3d-89f0-44e2-9536-913f11d4cf7e";
        public string CLAIMSPROCESS = "272b1065-9298-4d56-a6d9-fc33dcbd9916";
        public string ATTESTATIONSPROCESS = "7a7001d6-1f80-451b-809b-492255ea69b6";

        public HashSet<SiccarStatusCacheProcessResponse> ProcessResponses { get; set; }
        public HashSet<ProgressReport> ProgressReports { get; set; }

        public SiccarStatusCacheResponse()
        {
            ProcessResponses = new HashSet<SiccarStatusCacheProcessResponse>();
            ProgressReports = new HashSet<ProgressReport>();
        }
    }
}
