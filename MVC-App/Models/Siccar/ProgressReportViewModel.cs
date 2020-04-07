using Siccar.Shallow.Models;
using System;

namespace MVC_App.Models
{
    public class ProgressReportViewModel
    {
        public string Schema { get; set; }
        public string Title { get; set; }
        public System.Collections.Generic.List<ProgressReportStep> Steps { get; set; }

        public ProgressReportViewModel(ProgressReport parent)
        {
            this.Schema = parent.Schema;
            this.Steps = parent.Steps;
            this.Title = parent.Title;
        }
    }
}
