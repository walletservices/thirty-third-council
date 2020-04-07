using System;
using System.Collections.Generic;

namespace Siccar.Shallow.Models
{
    public class ProgressReport
    {

        public string Schema { get; set; }
        public string Title { get; set; }
        public List<ProgressReportStep> Steps { get; set; }

        public ProgressReport()
        {
            Steps = new List<ProgressReportStep>();
        }

        public void SetFirstStepToCompletedAndSecondToInProgress()
        {
            // This method is called when the user submits the first step and before Siccar has responded with success
            if (Steps.Count > 1)
            {
                Steps[0].SetStepToCompleted();
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
                    && p.Steps.Count == this.Steps.Count;
            }
        }

        public override int GetHashCode()
        {
            var schemeHashCode = Schema == null ? "".GetHashCode() : Schema.GetHashCode();
            var titleHashCode = Title == null ? "".GetHashCode() : Title.GetHashCode();

            return schemeHashCode
                + titleHashCode
                + Steps.GetHashCode();
        }

    }
}
