using System.Collections.Generic;
using MVC_App.Models;

namespace MVC_App.Cache.Caches
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