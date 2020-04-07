using Microsoft.Extensions.Caching.Memory;
using Siccar.Shallow.Models;
using siccar_cache.Caches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Siccar.CacheManager.Caches
{
    public class ProgressReportCache : AbstractCollectionCache<ProgressReport>, IProgressReportCache
    {
        private static string KEY_ID = "pr_";

        
        public ProgressReportCache(IMemoryCache cache) : base(cache)
        {
        }

        private string BuildKey(string id)
        {
            return $"{KEY_ID}{id}";
        }


        public HashSet<ProgressReport> GetReport(string userId)
        {
            return GetValues(BuildKey(userId));
        }

        public void AddProgressReports(string userId, HashSet<ProgressReport> reports)
        {
            AddValues(BuildKey(userId), reports);
        }

        public void AddProgressReport(string userId, ProgressReport report)
        {
            AddValue(BuildKey(userId), report);
        }
    }
}
