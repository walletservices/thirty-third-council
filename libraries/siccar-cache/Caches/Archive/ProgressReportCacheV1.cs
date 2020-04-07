//using Siccar.Shallow.Models;
//using System.Collections.Generic;
//using System.Linq;

//namespace Siccar.CacheManager.Caches
//{
//    public class ProgressReportCacheV1 : IProgressReportCache
//    {
//        private Dictionary<string, HashSet<ProgressReport>> _progressReports;


//        public ProgressReportCacheV1()
//        {
//            _progressReports = new Dictionary<string, HashSet<ProgressReport>>();
//        }

//        public HashSet<ProgressReport> GetReport(string userId)
//        {
//            if (_progressReports.ContainsKey(userId))
//            {
//                return _progressReports[userId];
//            }
//            return new HashSet<ProgressReport>();
//        }

//        public void AddProgressReports(string userId, HashSet<ProgressReport> reports)
//        {
//            if (_progressReports.ContainsKey(userId))
//            {
//                var hash = _progressReports[userId];

//                // Originally this used reports but it caused a concurrent exception
//                // Extracted to a to list which didnt change anything 
//                var hashReports = reports.ToList();
//                foreach (var report in hashReports)
//                {
//                    var found = _progressReports[userId].ToList().Find(x => x.Schema == report.Schema && x.Title == report.Title);
//                    if (found != null)
//                    {
//                        // Replace incase steps have changed
//                        hash.Remove(found);
//                    }
//                    hash.Add(report);
//                }
//                _progressReports[userId] = hash;
//            }
//            else
//            {
//                _progressReports.Add(userId, reports);
//            }
//        }

//        public void AddProgressReport(string userId, ProgressReport report)
//        {
//            if (_progressReports.ContainsKey(userId))
//            {
//                var hashsets = _progressReports[userId];
//                hashsets.Add(report);
//                _progressReports[userId] = hashsets;
//            }
//            else
//            {
//                _progressReports.Add(userId, new HashSet<ProgressReport>() { report });
//            }
//        }

//    }
//}
