using MVC_App.Siccar;
using Newtonsoft.Json;
using System;
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

        public async Task<string> GetProgressReport(string idToken)
        {
            return await _client.Get(_config.GetGetProgressReports, idToken);
        }

        public async Task<string> GetStepNextOrStartProcess(string schemaId, string schemaVersionId, string idToken)
        {
            var actionsToExecute = await _client.Get(_config.GetGetActionToLoad, idToken);
            if (actionsToExecute.Equals("[]"))
            {
                await _client.Post(_config.PostStartProcess, idToken, JsonConvert.SerializeObject(new { schemaId, schemaVersionId } as dynamic));
                actionsToExecute = await _client.Poll(_config.GetCheckifActionIsStartable, idToken);
            }
            var action = FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
            if (action == "-1")
            {
                await _client.Post(_config.PostStartProcess, idToken, JsonConvert.SerializeObject(new { schemaId, schemaVersionId } as dynamic));
                actionsToExecute = await _client.Poll(_config.GetCheckifActionIsStartable, idToken);
            }
            return FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
        }

        private string FindActionFromResponse(string actionsToExecute, string schemaId, string schemaVersionId)
        {
            try
            {
                dynamic potentialContent = JsonConvert.DeserializeObject<dynamic>(actionsToExecute);
                if (potentialContent is Newtonsoft.Json.Linq.JArray)
                {
                    foreach (var content in potentialContent)
                    {
                        if (content.summary.schemaId.Value == schemaId && content.summary.schemaVersionId.Value == schemaVersionId)
                        {
                            return content.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return "-1";
        }
    }
}
