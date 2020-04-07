using Siccar.Shallow.Models;
using System.Collections.Generic;

namespace Siccar.CacheManager.Caches
{
    public interface IProgressReportCache
    {
        void AddProgressReport(string userId, ProgressReport report);
        void AddProgressReports(string userId, HashSet<ProgressReport> reports);
        HashSet<ProgressReport> GetReport(string userId);
    }
}