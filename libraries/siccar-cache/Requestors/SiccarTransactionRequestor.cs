using Siccar.Connector.Connector;
using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
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
