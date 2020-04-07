using Siccar.CacheManager.Caches;
using Siccar.CacheManager.ModelManagers;
using Siccar.CacheManager.Models;
using Siccar.CacheManager.Requestors;
using Siccar.Connector.Connector;
using Siccar.Shallow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siccar.CacheManager
{
    public class SiccarStatusCache : ISiccarStatusCache
    {
        //Part 2 
        private ISiccarTransactionRequestor _transactionRequestor;
        private IProgressReportRequestor _progressReportRequestor;

        private IUserCache _userCache;
        private IUserJustCompletedStepCache _userJustCompletedStepCache;

        private ISiccarTransactionCache _siccarTransactionCache;
        private IProgressReportCache _progressReportCache;
        private IProcessSchemaCache _processSchemaCache;

        private ISiccarStatusCacheResponseManager _siccarStatusCacheResponseManager;
        private IProgressReportModelManager _progressReportModelManager;
        private ISiccarTransactionManager _siccarTransactionManager;


        public SiccarStatusCache(
            ISiccarTransactionRequestor transactionRequestor,
            IProgressReportRequestor progressReportRequestor,
            IUserCache userCache,
            ISiccarTransactionCache siccarTransactionCache,
            IProgressReportCache progressReportCache,
            IProcessSchemaCache processSchemaCache,
            IUserJustCompletedStepCache userJustCompletedStepCache,
            ISiccarStatusCacheResponseManager siccarStatusCacheResponseManager,
            IProgressReportModelManager progressReportModelManager,
            ISiccarTransactionManager siccarTransactionManager
            )
        {
            _transactionRequestor = transactionRequestor;
            _progressReportRequestor = progressReportRequestor;
            _userCache = userCache;
            _siccarTransactionCache = siccarTransactionCache;

            _progressReportModelManager = progressReportModelManager;

            _progressReportCache = progressReportCache;
            _processSchemaCache = processSchemaCache;
            _siccarStatusCacheResponseManager = siccarStatusCacheResponseManager;
            _siccarTransactionManager = siccarTransactionManager;
            _userJustCompletedStepCache = userJustCompletedStepCache;
        }

        public void ReflectSubmissionFromUser(string userId, string schemaId)
        {
            AddUserToJustCompletedStepCache(userId, schemaId);
            UpdateProgressReportToReflectSubmission(userId);
            UpdateStatususInCache(userId, _userCache.GetIdToken(userId));

        }

        public void RemoveUser(string userId)
        {
            _userCache.RemoveUser(userId);
        }

        // Replace this with a notifications hook 
        public async Task UpdateStatusInCacheForEveryUser()
        {
            Console.WriteLine("UpdateStatusInCacheForEveryUser");
            foreach (var p in _userCache.GetLoggedInUsers())
            {
                await UpdateStatususInCache(p, _userCache.GetIdToken(p));
            }
        }

        // Kick off method 
        public async Task RefreshAndDontWait(string guid, string idToken)
        {
            Console.WriteLine("RefreshAndDontWait");
            await Task.Run((() => UpdateStatususInCache(guid, idToken)));
        }

        public SiccarStatusCacheResponse GetStatus(string userId)
        {
            return _siccarStatusCacheResponseManager.BuildSiccarStatusCacheResponseForUser(userId);
        }

        public void UpdateProgressReportToReflectSubmission(string guid)
        {
            var reports = _progressReportCache.GetReport(guid);
            if (IsFirstTimeSubmission(guid, reports))
            {
                // First time submission
                _progressReportCache.AddProgressReport(guid, _progressReportModelManager.BuildTemplateProgressReport(guid));
            }
            else
            {
                reports.ToList().Find(x => x.Schema == _userJustCompletedStepCache.GetSchema(guid)).SetFirstStepToCompletedAndSecondToInProgress();
                _progressReportCache.AddProgressReports(guid, reports.ToHashSet());
            }
        }

        private bool IsFirstTimeSubmission(string guid, HashSet<ProgressReport> reports)
        {
            return reports.Count == 0 ||
                   _userJustCompletedStepCache.GetSchema(guid) == null ||
                   reports.ToList().Find(x => x.Schema == _userJustCompletedStepCache.GetSchema(guid)) == null;
        }

        private async Task UpdateStatususInCache(string guid, string idToken)
        {
            try
            {
                var progress = await _progressReportRequestor.FetchProgress(idToken);
                AddSchemas(guid, progress);
                AddUser(guid, idToken);
                await PopulateSiccarTransactionCacheWithTransactionsFromProgressReport(idToken, progress);
                UpdateProgressReport(guid, progress);

            }
            catch (Exception)
            {
                _userCache.AddUser(guid, idToken);
            }
        }

        private void UpdateProgressReport(string guid, HashSet<ProcessSchema> schemas)
        {
            if (schemas.Count > 0/* && !_processSchemaCache.ContainsAll(guid, schemas)*/)
            {
                AddProgressReportsFromProgressReport(guid, schemas);
                RemoveUserFromJustCompletedStepCache(guid);
            }
        }

        private void AddProgressReportsFromProgressReport(string guid, HashSet<ProcessSchema> schemas)
        {
            _progressReportCache.AddProgressReports(guid, _progressReportModelManager.BuildProgressReports(schemas).ToHashSet());
        }

        private void AddUser(string guid, string idToken)
        {
            _userCache.AddUser(guid, idToken);
        }

        private void AddSchemas(string guid, HashSet<ProcessSchema> schemas)
        {
            _processSchemaCache.AddSchemas(guid, schemas);
        }

        private async Task PopulateSiccarTransactionCacheWithTransactionsFromProgressReport(string idToken, HashSet<ProcessSchema> progress)
        {
            var txs = await _siccarTransactionManager.GetTransactionIdsFromProgressReport(progress);
            await PopulateSiccarTransactionCache(idToken, txs);
        }

        public void AddUserToJustCompletedStepCache(string userId, string schemaId)
        {
            _userJustCompletedStepCache.AddUser(userId, schemaId);
        }

        public void RemoveUserFromJustCompletedStepCache(string userId)
        {
            _userJustCompletedStepCache.RemoveUser(userId);
        }

        public bool HasUserBeenProcessed(string userId)
        {
            return _userCache.IsUserLoggedIn(userId);
        }

        public async Task PopulateSiccarTransactionCache(string idToken, List<string> txs)
        {

            var transactionsNeedingAdded = txs.Where(x => !_siccarTransactionCache.ContainsTransaction(x));
            var list = new List<Task>();
            foreach (var t in transactionsNeedingAdded)
            {
                Task tsk = Task.Run(() => AddTransactionToCache(t, idToken));
                list.Add(tsk);
            }
            await Task.WhenAll(list);
        }

        private async Task AddTransactionToCache(string transaction, string idToken)
        {
            var tx = await _transactionRequestor.FetchTransaction(idToken, transaction);
            var siccarTx = await _siccarTransactionManager.BuildSingleTransactionView(transaction, tx, idToken);
            _siccarTransactionCache.AddTransaction(siccarTx);
        }

    }
}
