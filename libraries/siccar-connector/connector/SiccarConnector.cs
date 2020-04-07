using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Siccar.Connector.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.Connector.Connector
{
    public class SiccarConnector : ISiccarConnector
    {
        private ISiccarEndpoints _config;
        private ISiccarHttpClient _client;
        private ILogger _logger;

        public SiccarConnector(ISiccarEndpoints config, ISiccarHttpClient client, ILogger logger)
        {
            _config = config;
            _client = client;
            _logger = logger;
        }

        public async Task<FileContentResult> GetDocumentTransaction(string idToken, string transactionId)
        {
            _logger.LogInformation("SiccarConnector GetDocumentTransaction called", new[] { idToken, transactionId });
            return await _client.GetDocument($"{_config.GetDocumentTransaction}{transactionId}", idToken);
        }

        public async Task<string> GetMeEndpoint(string idToken)
        {
            _logger.LogInformation("SiccarConnector GetMeEndpoint called", new[] { idToken });
            return await _client.Get(_config.GetMe, idToken);
        }

        public async Task<string> GetTransaction(string idToken, string transactionId)
        {
            _logger.LogInformation("SiccarConnector GetTransaction called", new[] { idToken, transactionId });
            return await _client.Get($"{_config.GetTransaction}{transactionId}", idToken);
        }

        public async Task<string> GetProgressReport(string idToken)
        {
            _logger.LogInformation("SiccarConnector GetProgressReport called", new[] { idToken });
            return await _client.Get(_config.GetGetProgressReports, idToken);
        }

        public async Task<string> GetAvailableActions(string idToken)
        {
            _logger.LogInformation("SiccarConnector GetAvailableActions called", new[] { idToken });
            return await _client.Get(_config.GetGetActionToLoad, idToken);
        }

        public async Task<string> GetStartableProcesses(string idToken)
        {
            _logger.LogInformation("SiccarConnector GetStartableProcesses called", new[] { idToken });
            return await _client.Get(_config.GetStartableProcesses, idToken);
        }

        public async Task<string> SubmitStep(dynamic content, string idToken, string stepId)
        {
            _logger.LogInformation("SiccarConnector SubmitStep called", new[] { content, idToken, stepId });
            var stringContent = JsonConvert.SerializeObject(content);
            var endpoint = _config.PostSubmitStep.Replace("{id}", stepId);
            return await _client.Post(endpoint, idToken, stringContent, true);
        }

        public async Task<string> GetSchemaVersionFromStartableProcess(string schemaId, string idToken)
        {
            _logger.LogInformation("SiccarConnector GetSchemaVersionFromStartableProcess called", new[] { schemaId, idToken });
            var processesThatAreStartable = await GetStartableProcesses(idToken);
            if (processesThatAreStartable.Equals("[]"))
            {
                _logger.LogInformation("SiccarConnector GetSchemaVersionFromStartableProcess: No Startable Processes for user");
                return "-1";
            }

            dynamic potentialContent = JsonConvert.DeserializeObject<dynamic>(processesThatAreStartable);
            if (potentialContent is JArray)
            {
                foreach (var content in potentialContent)
                {
                    if (content.id == schemaId)
                    {
                        _logger.LogInformation("SiccarConnector GetSchemaVersionFromStartableProcess: Found a suiltable schema version to return", new[] { schemaId, content.version });
                        return content.version;
                    }
                }
            }
            _logger.LogInformation("SiccarConnector GetSchemaVersionFromStartableProcess: Did not find a matching schema", new[] { schemaId });
            return "-1";
        }

        public async Task<string> GetStepNextOrStartProcess(string idToken, string schemaId, string schemaVersionId = null, List<string> tokens = null)
        {
            _logger.LogInformation("SiccarConnector GetStepNextOrStartProcess  called", new[] { idToken, schemaId, schemaVersionId });
            var actionsToExecute = await GetAvailableActions(idToken);

            schemaVersionId = schemaVersionId ?? await GetSchemaVersionFromStartableProcess(schemaId, idToken);
            var action = FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
            if (action == "-1")
            {
                // No actions exist 
                _logger.LogInformation("SiccarConnector GetStepNextOrStartProcess: No existing actions, starting new process");
                await _client.Post(_config.PostStartProcess, idToken, JsonConvert.SerializeObject(new { schemaId, schemaVersionId } as dynamic), tokens);
                actionsToExecute = await _client.Poll(_config.GetCheckifActionIsStartable, idToken);
            }
            return FindActionFromResponse(actionsToExecute, schemaId, schemaVersionId);
        }



        private string FindActionFromResponse(string actionsToExecute, string schemaId, string schemaVersionId)
        {
            _logger.LogInformation("SiccarConnector FindActionFromResponse  called", new[] { actionsToExecute, schemaId, schemaVersionId });
            try
            {
                dynamic potentialContent = JsonConvert.DeserializeObject<dynamic>(actionsToExecute);
                if (potentialContent is Newtonsoft.Json.Linq.JArray)
                {
                    foreach (var content in potentialContent)
                    {
                        if (content.summary.schemaId.Value == schemaId && content.summary.schemaVersionId.Value == schemaVersionId)
                        {
                            _logger.LogInformation("SiccarConnector FindActionFromResponse: Found matching action");
                            return content.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            _logger.LogInformation("SiccarConnector FindActionFromResponse: Did not find matching action");
            return "-1";
        }
    }
}
