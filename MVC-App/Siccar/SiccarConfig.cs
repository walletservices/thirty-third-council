using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public class SiccarConfig : ISiccarConfig
    {
        public string ProcessA { get; set; }
        public string ProcessB { get; set; }
        public string ProcessC { get; set; }
        public string GetGetActionToLoad { get; set; }
        public string GetCheckifActionIsStartable { get; set; }
        public string GetProcessesThatICanStart { get; set; }
        public string PostStartProcess { get; set; }
        public string PostSubmitStep { get; set; }
        public string GetGetProgressReports { get; set; }
    }
}
