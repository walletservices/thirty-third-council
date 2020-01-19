using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MVC_App.ProcessModel;

namespace MVC_App.Cache.Requestors
{
    public class ProgressReportRequestor : IProgressReportRequestor
    {
        private ISiccarConnector _connector;

        public ProgressReportRequestor(ISiccarConnector connector)
        {
            _connector = connector;
        }

        public async Task<HashSet<ProcessSchema>> FetchProgress(string idToken)
        {
            var response = await _connector.GetProgressReport(idToken);
            JArray jsonObject = JArray.Parse(response);
            return jsonObject.ToObject<HashSet<ProcessSchema>>();
        }

    }
}
