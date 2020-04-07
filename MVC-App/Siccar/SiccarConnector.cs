using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var progressreport = await _client.Get(_config.GetGetProgressReports, idToken);
            return progressreport;
        }

        public async Task<string> GetStepNextOrStartProcess(string schemaId, string schemaVersionId, string idToken, string attestationToken = null)
        {
            var actionsToExecute = await _client.Get(_config.GetGetActionToLoad, idToken);
            if (actionsToExecute.Equals("[]"))
            {
                await _client.Post(_config.PostStartProcess, idToken, JsonConvert.SerializeObject(new { schemaId, schemaVersionId } as dynamic), attestationToken);
                actionsToExecute = await _client.Poll(_config.GetCheckifActionIsStartable, idToken);
            }
            var action = FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
            if (action == "-1")
            {
                await _client.Post(_config.PostStartProcess, idToken, JsonConvert.SerializeObject(new { schemaId, schemaVersionId } as dynamic), attestationToken);
                actionsToExecute = await _client.Poll(_config.GetCheckifActionIsStartable, idToken);
            }
            return FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
        }

        public async Task<string> SubmitStep(dynamic content, string idToken, string stepId)
        {
            var stringContent = JsonConvert.SerializeObject(content);
            var endpoint = _config.PostSubmitStep.Replace("{id}", stepId);
            await _client.Post(endpoint, idToken, stringContent,true);
            return "done";
        }

        public async Task<string> GetTransaction(string idToken, string transactionId)
        {
            return await _client.Get($"{_config.GetTransaction}{transactionId}", idToken);
        }

        public async Task<FileContentResult> GetDocumentTransaction(string idToken, string transactionId)
        {
            return await _client.GetDocument($"{_config.GetDocumentTransaction}{transactionId}", idToken);
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
