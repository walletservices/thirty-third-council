
using Siccar.CacheManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace MVC_App.Models
{
    public class SiccarStatusCacheResponseViewModel
    {
        public string DISABLEDBLUEBADGEPROCESS = "12374e3d-89f0-44e2-9536-913f11d4cf7e";
        public string CLAIMSPROCESS = "272b1065-9298-4d56-a6d9-fc33dcbd9916";
        public string ATTESTATIONSPROCESS = "7a7001d6-1f80-451b-809b-492255ea69b6";

        public HashSet<SiccarStatusCacheProcessResponseViewModel> ProcessResponses { get; set; }
        public HashSet<ProgressReportViewModel> ProgressReports { get; set; }

        public SiccarStatusCacheResponseViewModel()
        {
            ProcessResponses = new HashSet<SiccarStatusCacheProcessResponseViewModel>();
            ProgressReports = new HashSet<ProgressReportViewModel>();
        }

        public SiccarStatusCacheResponseViewModel(SiccarStatusCacheResponse parent)
        {
            this.ProgressReports = parent.ProgressReports.Select(x => new ProgressReportViewModel(x)).ToHashSet();
            this.ProcessResponses = parent.ProcessResponses.Select(x => new SiccarStatusCacheProcessResponseViewModel(x)).ToHashSet();
        }
    }
}
