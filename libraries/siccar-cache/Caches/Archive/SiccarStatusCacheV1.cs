//using Siccar.CacheManager.Caches;
//using Siccar.CacheManager.ModelManagers;
//using Siccar.CacheManager.Models;
//using Siccar.CacheManager.Requestors;
//using Siccar.Connector.Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Siccar.CacheManager
//{
//    public class SiccarStatusCacheV1 : ISiccarStatusCache
//    {
//        private readonly ISiccarConnector _connector;

//        //Part 2 
//        private ISiccarTransactionRequestor _transactionRequestor;
//        private IProgressReportRequestor _progressReportRequestor;

//        private IUserCache _userCache;
//        private IUserJustCompletedStepCache _userJustCompletedStepCache;

//        private ISiccarTransactionCache _siccarTransactionCache;
//        private IProgressReportCache _progressReportCache;
//        private IProcessSchemaCache _processSchemaCache;

//        private ISiccarStatusCacheResponseManager _siccarStatusCacheResponseManager;
//        private IProgressReportModelManager _progressReportModelManager;
//        private ISiccarTransactionManager _siccarTransactionManager;


//        public SiccarStatusCache(
//            ISiccarConnector connector,
//            ISiccarTransactionRequestor transactionRequestor,
//            IProgressReportRequestor progressReportRequestor,
//            IUserCache userCache,
//            ISiccarTransactionCache siccarTransactionCache,
//            IProgressReportCache progressReportCache,
//            IProcessSchemaCache processSchemaCache,
//            IUserJustCompletedStepCache userJustCompletedStepCache,
//            ISiccarStatusCacheResponseManager siccarStatusCacheResponseManager,
//            IProgressReportModelManager progressReportModelManager,
//            ISiccarTransactionManager siccarTransactionManager
//            )
//        {
//            _connector = connector;
//            _transactionRequestor = transactionRequestor;
//            _progressReportRequestor = progressReportRequestor;
//            _userCache = userCache;
//            _siccarTransactionCache = siccarTransactionCache;

//            _progressReportModelManager = progressReportModelManager;

//            _progressReportCache = progressReportCache;
//            _processSchemaCache = processSchemaCache;
//            _siccarStatusCacheResponseManager = siccarStatusCacheResponseManager;
//            _siccarTransactionManager = siccarTransactionManager;
//            _userJustCompletedStepCache = userJustCompletedStepCache;
//        }

//        public void ReflectSubmissionFromUser(string userId, string schemaId)
//        {
//            AddUserToJustCompletedStepCache(userId, schemaId);
//            UpdateProgressReportToReflectSubmission(userId);
//            UpdateStatususInCache(userId, _userCache.GetIdToken(userId));

//        }

//        public void RemoveUser(string userId)
//        {
//            _userCache.RemoveUser(userId);
//        }

//        // Replace this with a notifications hook 
//        public async Task UpdateStatusInCacheForEveryUser()
//        {
//            Console.WriteLine("UpdateStatusInCacheForEveryUser");
//            foreach (var p in _userCache.GetLoggedInUsers())
//            {
//                await UpdateStatususInCache(p, _userCache.GetIdToken(p));
//            }
//        }

//        // Kick off method 
//        public async Task RefreshAndDontWait(string guid, string idToken)
//        {
//            Console.WriteLine("RefreshAndDontWait");
//            await Task.Run((() => UpdateStatususInCache(guid, idToken)));
//        }

//        public SiccarStatusCacheResponse GetStatus(string userId)
//        {
//            return _siccarStatusCacheResponseManager.BuildSiccarStatusCacheResponseForUser(userId);
//        }

//        public void UpdateProgressReportToReflectSubmission(string guid)
//        {
//            var reports = _progressReportCache.GetReport(guid);
//            if (reports.Count == 0 ||
//                _userJustCompletedStepCache.GetSchema(guid) == null ||
//                reports.ToList().Find(x => x.Schema == _userJustCompletedStepCache.GetSchema(guid)) == null)
//            {
//                // First time submission
//                _progressReportCache.AddProgressReport(guid, _progressReportModelManager.BuildTemplateProgressReport(guid));
//            }
//            else
//            {
//                reports.ToList().Find(x => x.Schema == _userJustCompletedStepCache.GetSchema(guid)).SetFirstStepToCompletedAndSecondToInProgress();
//                _progressReportCache.AddProgressReports(guid, reports.ToHashSet());
//            }

//        }

//        private async Task UpdateStatususInCache(string guid, string idToken)
//        {
//            try
//            {

//                var progress = await _progressReportRequestor.FetchProgress(idToken);

//                // This will only update every time the schema changes 
//                // All it does is fetch the progress report and then transcations if needed
//                if (progress.Count > 0 && !_processSchemaCache.ContainsAll(guid, progress))
//                {
//                    var reports = _progressReportModelManager.BuildProgressReports(progress);
//                    var txs = await _siccarTransactionManager.GetTransactionIdsFromProgressReport(progress);
//                    await PopulateSiccarTransactionCache(idToken, txs);
//                    _processSchemaCache.AddSchemas(guid, progress);
//                    _progressReportCache.AddProgressReports(guid, reports.ToHashSet());
//                    // Add user after
//                    _userCache.AddUser(guid, idToken);
//                    RemoveUserFromJustCompletedStepCache(guid);
//                    Console.WriteLine("Updated");
//                }
//                else if (progress.Count == 0)
//                {
//                    _userCache.AddUser(guid, idToken);
//                }
//                else
//                {

//                    Console.WriteLine("No change detected");
//                }
//            }
//            catch (Exception)
//            {
//                if (!_userCache.IsUserLoggedIn(guid))
//                {
//                    _userCache.AddUser(guid, idToken);
//                }
//            }
//        }

//        public void AddUserToJustCompletedStepCache(string userId, string schemaId)
//        {
//            _userJustCompletedStepCache.AddUser(userId, schemaId);
//        }

//        public void RemoveUserFromJustCompletedStepCache(string userId)
//        {
//            _userJustCompletedStepCache.RemoveUser(userId);
//        }

//        public bool HasUserBeenProcessed(string userId)
//        {
//            return _userCache.IsUserLoggedIn(userId);
//        }

//        public async Task PopulateSiccarTransactionCache(string idToken, List<string> txs)
//        {
//            foreach (var tx in txs)
//            {
//                if (!_siccarTransactionCache.ContainsTransaction(tx))
//                {
//                    var siccarTransactionString = await _transactionRequestor.FetchTransaction(idToken, tx);
//                    var siccarTx = await _siccarTransactionManager.BuildSingleTransactionView(tx, siccarTransactionString, idToken);
//                    _siccarTransactionCache.AddTransaction(siccarTx);
//                }
//            }
//        }


//    }
//}
