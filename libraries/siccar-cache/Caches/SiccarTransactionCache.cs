using Microsoft.Extensions.Caching.Memory;
using Siccar.Shallow.Models;
using siccar_cache.Caches;
using System.Collections.Generic;
using System.Linq;

namespace Siccar.CacheManager.Caches
{
    public class SiccarTransactionCache : AbstractCache<string, SiccarTransaction>, ISiccarTransactionCache
    {
        private static string KEY_ID = "tx_";

        public SiccarTransactionCache(IMemoryCache cache) : base(cache)
        {
        }

        private string BuildKey(string id)
        {
            return $"{KEY_ID}{id}";
        }



        public bool ContainsTransaction(string transactionId)
        {
            return KeyExistsInCache(BuildKey(transactionId));
        }


        public HashSet<SiccarTransaction> GetTransactions()
        {
            return GetAll().ToHashSet();
        }

        public SiccarTransaction GetTransaction(string id)
        {
            return GetValue(BuildKey(id));
        }

        public void AddTransactions(HashSet<SiccarTransaction> transactions)
        {
            foreach (var tx in transactions)
            {
                AddValue(BuildKey(tx.TransactionId), tx);
            }
        }

        public void AddTransaction(SiccarTransaction transaction)
        {
            AddValue(BuildKey(transaction.TransactionId), transaction);
        }
    }
}
