using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Models
{
    public class ProgressReport
    {
        public ProgressReport()
        {
            Steps = new List<ProgressReportStep>();
        }


        public string Schema { get; set; }
        public string Title { get; set; }
        public List<ProgressReportStep> Steps { get; set; }

        public void SetFirstStepToCompletedAndSecondToInProgress()
        {
            // This method is called when the user submits the first step and before Siccar has responded with success
            if (Steps.Count > 1)
            {
                Steps[0].SetSetToCompleted();
                Steps[1].SetStepToInProgress();
            }
        }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ProgressReport p = (ProgressReport)obj;
                return p.Schema == this.Schema
                    && p.Title == this.Title
                    && p.Steps == this.Steps;
            }
        }

        public override int GetHashCode()
        {
            return Schema.GetHashCode()
                + Title.GetHashCode()
                + Steps.GetHashCode();
        }

    }
}
