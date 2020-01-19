using MVC_App.Cache.Caches;
using MVC_App.Models;
using System.Collections.Generic;
using System.Linq;
using static MVC_App.ProcessModel;

namespace MVC_App.Cache
{
    public class SiccarStatusCacheResponseManager : ISiccarStatusCacheResponseManager
    {
        private IUserCache _userCache;
        private ISiccarTransactionCache _siccarTransactionCache;
        private IProgressReportCache _progressReportCache;
        private IProcessSchemaCache _processSchemaCache;
        private IUserJustCompletedStepCache _userJustCompletedStepCache;

        public SiccarStatusCacheResponseManager(IUserCache userCache, ISiccarTransactionCache siccarTransactionCache, IProgressReportCache progressReportCache, IProcessSchemaCache processSchemaCache, IUserJustCompletedStepCache userJustCompletedStepCache)
        {
            _userCache = userCache;
            _siccarTransactionCache = siccarTransactionCache;
            _progressReportCache = progressReportCache;
            _processSchemaCache = processSchemaCache;
            _userJustCompletedStepCache = userJustCompletedStepCache;
        }

        public SiccarStatusCacheResponse BuildSiccarStatusCacheResponseForUser(string userId)
        {
            var responses = new List<SiccarStatusCacheProcessResponse>();
            var schemas = _processSchemaCache.GetSchemas(userId);
            foreach (var schema in schemas)
            {
                var attributes = new List<KeyValuePair<string, string>>();

                // The first tx is actually the last the way the results from the server
                var authorisedTransaction = schema.stepStatuses[0].stepTransactionId;
                if (authorisedTransaction != null)
                {
                    var fatributes = _siccarTransactionCache.GetTransaction(authorisedTransaction).Attributes;
                    attributes.AddRange(fatributes);

                    // Only add the last transactions if the first ones have been added 
                    var userApplicationTransaction = schema.stepStatuses[schema.stepStatuses.Count - 1].stepTransactionId;
                    if (userApplicationTransaction != null)
                    {
                        var uatributes = _siccarTransactionCache.GetTransaction(userApplicationTransaction).Attributes;
                        attributes.AddRange(uatributes);
                    }
                }
                var status = GetStatusOfProcess(schema, userId);
                var atrs = attributes.ToHashSet().ToList();
                var schemaId = schema.schemaId;
                var statusCacheResponse = new SiccarStatusCacheProcessResponse(schemaId, status, atrs);
                responses.Add(statusCacheResponse);
            }
            
            

            var response = new SiccarStatusCacheResponse()
            {
                ProgressReports = _progressReportCache.GetReport(userId),
                ProcessResponses = responses.ToHashSet()
            };

            return response;
        }

        private ProcessStatus GetStatusOfProcess(ProcessSchema schema, string userId)
        {
            if (schema != null)
            {
                if (_userJustCompletedStepCache.UserAndSchemaStored(userId, schema.schemaId)) {
                    return ProcessStatus.IN_PROGRESS;
                }
                var statusus = schema.stepStatuses;
                if (statusus[0].completionTime != null)
                {
                    return ProcessStatus.COMPLETED;
                }
                if (statusus[statusus.Count - 1].completionTime != null)
                {
                    return ProcessStatus.IN_PROGRESS; 
                }
            }
            return ProcessStatus.NOT_STARTED;
        }
    }
}
