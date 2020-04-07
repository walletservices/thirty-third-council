using Siccar.Shallow.Models;
using System.Collections.Generic;

namespace Siccar.CacheManager.ModelManagers
{
    public interface IProgressReportModelManager
    {
        List<ProgressReport> BuildProgressReports(HashSet<ProcessSchema> schemas);

        ProgressReport BuildTemplateProgressReport(string schemaId);
    }
}