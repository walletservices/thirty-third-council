using Siccar.Connector.Connector;
using System;
using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
{
    public class DocumentRequestor : IDocumentRequestor
    {
        private ISiccarConnector _connector;

        public DocumentRequestor(ISiccarConnector connector)
        {
            _connector = connector;
        }

        public async Task<string> FetchDocument(string idToken, string transactionHash)
        {
            var response = await _connector.GetDocumentTransaction(idToken, transactionHash);
            var contents = Convert.ToBase64String(response.FileContents);
            return contents;
        }
    }
}
