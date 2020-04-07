//using Siccar.Shallow.Models;
//using System.Collections.Generic;
//using System.Linq;

//namespace Siccar.CacheManager.Caches
//{
//    public class SiccarTransactionCacheV1 : ISiccarTransactionCache
//    {
//        private HashSet<SiccarTransaction> _transactions;

//        public SiccarTransactionCacheV1()
//        {
//            _transactions = new HashSet<SiccarTransaction>();
//        }

//        public bool ContainsTransaction(string transactionId)
//        {
//            return _transactions.Contains(new SiccarTransaction() { TransactionId = transactionId });

//        }


//        public HashSet<SiccarTransaction> GetTransactions()
//        {
//            return _transactions;
//        }

//        public SiccarTransaction GetTransaction(string id)
//        {
//            return _transactions.ToList().Find(x => x.TransactionId == id);
//        }

//        public void AddTransactions(HashSet<SiccarTransaction> transactions)
//        {
//            // Found a bug here which prevented the transactiosn being added.
//            // Fixed the code if no bug found by April 202 delete this comment
//            foreach (var tx in transactions)
//            {
//                _transactions.Add(tx);
//            }
//        }

//        public void AddTransaction(SiccarTransaction transaction)
//        {
//            _transactions.Add(transaction);
//        }
//    }
//}
