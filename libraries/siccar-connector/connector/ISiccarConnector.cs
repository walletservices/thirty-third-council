using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.Connector.Connector
{
    public interface ISiccarConnector
    {
        /// <summary>
        /// if version is not passed to uit, it will fetch the latest. 
        /// Requires the SiccarEndpoint object to exist and the GetStartableProcesses string to point at the 
        /// /processes/available endpoint on Siccar
        /// </summary>
        /// <param name="processid"></param>
        /// <param name="version"></param>
        /// <param name="idToken"></param>
        /// <param name="secondaryTokens"></param>
        /// <returns>The action to submit</returns>
        Task<string> GetStepNextOrStartProcess(string idToken, string processid, string version = null, List<string> tokens = null);

        /// <summary>
        /// Submits a step to Siccar
        /// Requires
        /// SiccarEndpoints.PostSubmitStep to be populated and point at 
        /// /registers/{registerId}/steps{id}/next
        /// </summary>
        /// <param name="content"></param>
        /// <param name="idToken"></param>
        /// <param name="transactionId"></param>
        /// <returns>Any string returned from the post</returns>
        Task<string> SubmitStep(dynamic content, string idToken, string transactionId);

        /// <summary>
        /// Returns the processes that can be started by the current user
        /// Requires
        /// SiccarEndpoints.GetStartableProcesses to be populated and point at 
        /// /registers/{registerId}/processes/available
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns>The processes that can be started</returns>
        Task<string> GetStartableProcesses(string idToken);

        /// <summary>
        /// Returns the latest schemaversion for the process passed to it
        /// Requires
        /// SiccarEndpoints.GetStartableProcesses to be populated and point at 
        /// /registers/{registerId}/processes/available
        /// </summary>
        /// <param name="schemaId"></param>
        /// <param name="idToken"></param>
        /// <returns>The latest schema version</returns>
        Task<string> GetSchemaVersionFromStartableProcess(string schemaId, string idToken);

        /// <summary>
        /// Returns the  available actions for the user
        /// Please note, depending on the user calling this the results will be different
        /// 
        /// Requires
        /// SiccarEndpoints.GetGetActionToLoad to be populated and point at 
        /// /registers/{registerId}/steps/next
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns>The available actions</returns>
        Task<string> GetAvailableActions(string idToken);


        /// <summary>
        /// Returns the  progress reports for the user calling it
        /// Please note, depending on the user calling this the results will be different
        /// 
        /// Requires
        /// SiccarEndpoints.GetGetProgressReports to be populated and point at 
        /// registers/{registerId}/reporting/progress
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns>The progress Reports</returns>
        Task<string> GetProgressReport(string idToken);

        /// <summary>
        /// Returns the  result of the transaction endpoint
        /// Please note, depending on the user calling this the results will be different
        /// 
        /// Requires
        /// SiccarEndpoints.GetTransaction to be populated and point at 
        /// /registers/{registerId}/transactions/{id}
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="transactionId"></param>
        /// <returns>The transaction</returns>
        Task<string> GetTransaction(string idToken, string transactionId);

        /// <summary>
        /// Returns the  result of the ME endpoint
        /// 
        /// Requires
        /// SiccarEndpoints.GetMe to be populated and point at 
        /// /registers/{registerId}/me
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns>The results of the Me endpoint</returns>
        Task<string> GetMeEndpoint(string idToken);

        /// <summary>
        /// Returns the  document stored in the transactionId passed to it
        /// 
        /// Requires
        /// SiccarEndpoints.GetDocumentTransaction to be populated and point at 
        /// /registers/{registerId}/storage/documents/{transactionId}
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="transactionId"></param>
        /// <returns>The document transaction</returns>
        Task<FileContentResult> GetDocumentTransaction(string idToken, string transactionId);

    }
}
