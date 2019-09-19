using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public interface ISiccarConfig
    {
        string ProcessA { get; set; }
        string ProcessB { get; set; }
        string ProcessC { get; set; }
        string GetGetActionToLoad { get; set; }
        string GetCheckifActionIsStartable { get; set; }
        string GetProcessesThatICanStart { get; set; }
        string PostStartProcess { get; set; }
        string PostSubmitStep { get; set; }
    }
}
