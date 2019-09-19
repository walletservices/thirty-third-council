using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public interface ISiccarConnector
    {
        string GetStepNextOrStartProcess(string processid, string idToken);

        string GetProgressReport(string idToken);
        
    }
}
