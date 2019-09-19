using MVC_App.Siccar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVC_App
{
    public class SiccarConnector : ISiccarConnector
    {

        private ISiccarConfig _config;

        public SiccarConnector(ISiccarConfig config)
        {
            _config = config;
        }

        public string GetProgressReport(string idToken)
        {
            return "Progress report";
        }

        public string GetStepNextOrStartProcess(string processid, string idToken)
        {
            return "GetStepNextOrstartProcess";
        }
    }
}
