using MVC_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MVC_App.ProcessModel;

namespace MVC_App.Cache.V2
{
    public class ProgressReportModelManager : IProgressReportModelManager
    {

        public ProgressReport BuildTemplateProgressReport(string schemaId)
        {
            return new ProgressReport { Title = "Application has just been submitted, please wait for confirmation of received", Schema = schemaId };
        }

        public List<ProgressReport> BuildProgressReports(HashSet<ProcessSchema> schemas)
        {
            var list = new List<ProgressReport>();
            foreach (var v in schemas)
            {
                var pr = new ProgressReport { Title = v.schemaTitle, Schema = v.schemaId };
                var statuses = v.stepStatuses;
                for (var i = 0; i < statuses.Count; i++)
                {
                    statuses[i].stepIndex = i;
                }
                statuses.Reverse();
                var potentialStatus = statuses.Find(x => x.completionTime == null);
                var stepInProgress = potentialStatus == null ? -1 : potentialStatus.stepIndex;
                pr.Steps.AddRange(statuses.Select(r => new ProgressReportStep(r, stepInProgress)));
                pr.Steps.Reverse();
                list.Add(pr);
            }
            return list;
        }
    }
}