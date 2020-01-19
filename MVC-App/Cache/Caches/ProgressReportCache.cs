using MVC_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Cache.Caches
{
    public class ProgressReportCache : IProgressReportCache
    {
        private Dictionary<string, HashSet<ProgressReport>> _progressReports;


        public ProgressReportCache()
        {
            _progressReports = new Dictionary<string, HashSet<ProgressReport>>();
        }

        public HashSet<ProgressReport> GetReport(string userId)
        {
            if (_progressReports.ContainsKey(userId))
            {
                return _progressReports[userId];
            }
            return new HashSet<ProgressReport>();
        }

        public void AddProgressReports(string userId, HashSet<ProgressReport> reports)
        {
            if (_progressReports.ContainsKey(userId))
            {
                //var hash = _progressReports[userId];
                //hash.ToList().AddRange(reports);
                //_progressReports[userId] = hash.ToHashSet();
                var hash = _progressReports[userId];

                foreach (var report in reports)
                {
                    var found = _progressReports[userId].ToList().Find(x => x.Schema == report.Schema);
                    if (found != null)
                    {
                        // Replace incase steps have changed
                        hash.Remove(found);
                    }
                    hash.Add(report);
                }
                _progressReports[userId] = hash;
            }
            else
            {
                _progressReports.Add(userId, reports);
            }
        }

        public void AddProgressReport(string userId, ProgressReport report)
        {
            if (_progressReports.ContainsKey(userId))
            {
                var hashsets = _progressReports[userId];
                hashsets.Add(report);
                _progressReports[userId] = hashsets;
            }
            else
            {
                _progressReports.Add(userId, new HashSet<ProgressReport>() {  report });
            }
        }

    }
}
