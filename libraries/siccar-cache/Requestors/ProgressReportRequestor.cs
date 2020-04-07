using Newtonsoft.Json.Linq;
using Siccar.Connector.Connector;
using Siccar.Shallow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
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
