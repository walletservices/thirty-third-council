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
        private ISiccarHttpClient _client;

        public SiccarConnector(ISiccarConfig config, ISiccarHttpClient client)
        {
            _config = config;
            _client = client;
        }

        public string GetProgressReport(string idToken)
        {
            return _client.Get(_config.GetGetProgressReports, idToken);
        }

        public string GetStepNextOrStartProcess(string processid, string idToken)
        {
            return "GetStepNextOrstartProcess";
        }
    }
}
