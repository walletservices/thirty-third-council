using System;

namespace Siccar.Shallow.Models
{
    public class ProgressReportStep
    {
        public static readonly string COMPLETED = "Completed";
        public static readonly string TODO = "To Do";
        public static readonly string INPROGRESS = "In Progress";
        public static readonly string ACTIVE = "active";

        public string Status { get; set; }
        public string Class { get; set; }
        public string Icon { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public string CompletionTime { get; set; }

        public ProgressReportStep()
        {

        }

        public ProgressReportStep(StepStatus status)
        {
            Index = 0;
            Title = status.stepTitle;
            CompletionTime = BuildDateTime(status.completionTime);
            SetStepToInProgress();
        }

        public ProgressReportStep(StepStatus status, int stepIndex, bool userInCache = false)
        {
            Index = status.stepIndex;
            Title = status.stepTitle;
            CompletionTime = BuildDateTime(status.completionTime);
            Status = BuildStatus(status.completionTime, (status.stepIndex == stepIndex || userInCache));
            Class = BuildClass(Status);
            Icon = BuildIconClasses(Status);
        }


        public void SetStepToCompleted()
        {
            Status = COMPLETED;
            Class = BuildClass(Status);
            Icon = BuildIconClasses(Status);
        }

        public void SetStepToInProgress()
        {
            Status = INPROGRESS;
            Class = BuildClass(Status);
            Icon = BuildIconClasses(Status);
        }
        public void SetStepToDo()
        {
            Status = TODO;
            Class = BuildClass(Status);
            Icon = BuildIconClasses(Status);
        }

        private string BuildIconClasses(string Status)
        {
            return Status == COMPLETED ? "green icon" : Status == TODO ? "cogs red icon" : "orange play icon";
        }

        private string BuildClass(string status)
        {
            return Status == COMPLETED ? COMPLETED.ToLower() : ACTIVE;
        }

        private string BuildStatus(int? completionTime, bool inProgress)
        {
            return inProgress ? INPROGRESS : completionTime == null ? TODO : COMPLETED;
        }

        private string BuildDateTime(int? time)
        {
            if (time == null)
            {
                return "N/A";
            }
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddSeconds(Convert.ToDouble(Convert.ToInt32(time)));
            return epoch.ToString("dd/MM/yyyy hh:mm:ss");
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
                ProgressReportStep p = (ProgressReportStep)obj;
                return p.CompletionTime == this.CompletionTime
                    && p.Icon == this.Icon
                    && p.Status == this.Status
                    && p.Title == this.Title
                    && p.Index == this.Index;
            }
        }

        public override int GetHashCode()
        {
            var statusHashCode = Status == null ? "".GetHashCode() : Status.GetHashCode();
            var TitleHashCode = Title == null ? "".GetHashCode() : Title.GetHashCode();
            var CompletionTimeHashCode = CompletionTime == null ? "".GetHashCode() : CompletionTime.GetHashCode();
            var IconHashCode = Icon == null ? "".GetHashCode() : Icon.GetHashCode();

            return statusHashCode + TitleHashCode + CompletionTimeHashCode + IconHashCode + Index.GetHashCode();
        }
    }
}

