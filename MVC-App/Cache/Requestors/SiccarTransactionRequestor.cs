using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Cache.Requestors
{
    public class SiccarTransactionRequestor : ISiccarTransactionRequestor
    {
        private ISiccarConnector _connector;

        public SiccarTransactionRequestor(ISiccarConnector connector)
        {
            _connector = connector;
        }

        public async Task<string> FetchTransaction(string idToken, string transactionHash)
        {
            return await _connector.GetTransaction(idToken, transactionHash);
        }
    }
}
