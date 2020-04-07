using Siccar.Shallow.Models;
using System.Collections.Generic;

namespace Siccar.CacheManager.Caches
{
    public interface ISiccarTransactionCache
    {
        void AddTransaction(SiccarTransaction transaction);
        void AddTransactions(HashSet<SiccarTransaction> transactions);
        bool ContainsTransaction(string transactionId);
        HashSet<SiccarTransaction> GetTransactions();
        SiccarTransaction GetTransaction(string id);
    }
}