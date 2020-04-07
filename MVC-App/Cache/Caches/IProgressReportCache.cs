using System.Collections.Generic;
using MVC_App.Models;

namespace MVC_App.Cache.Caches
{
    public interface IProgressReportCache
    {
        void AddProgressReport(string userId, ProgressReport report);
        void AddProgressReports(string userId, HashSet<ProgressReport> reports);
        HashSet<ProgressReport> GetReport(string userId);
    }
}