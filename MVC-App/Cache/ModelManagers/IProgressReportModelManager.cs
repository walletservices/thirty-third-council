using System.Collections.Generic;
using MVC_App.Models;

namespace MVC_App.Cache.V2
{
    public interface IProgressReportModelManager
    {
        List<ProgressReport> BuildProgressReports(HashSet<ProcessModel.ProcessSchema> schemas);

        ProgressReport BuildTemplateProgressReport(string schemaId);
    }
}