using MVC_App.Cache.Requestors;
using MVC_App.Cache.V2;
using MVC_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Cache.Caches
{
    public class SiccarTransactionCache : ISiccarTransactionCache
    {
        private HashSet<SiccarTransaction> _transactions;

        public SiccarTransactionCache()
        {
            _transactions = new HashSet<SiccarTransaction>();
        }

        public bool ContainsTransaction(string transactionId)
        {        
            return _transactions.Contains(new SiccarTransaction() { TransactionId = transactionId });

        }


        public HashSet<SiccarTransaction> GetTransactions()
        {
            return _transactions;
        }

        public SiccarTransaction GetTransaction(string id)
        {
            return _transactions.ToList().Find(x => x.TransactionId == id);
        }

        public void AddTransactions(HashSet<SiccarTransaction> transactions)
        {
            _transactions.ToList().AddRange(transactions);
        }

        public void AddTransaction(SiccarTransaction transaction)
        {
            _transactions.Add(transaction);
        }
    }
}
