using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App.Cache.Requestors
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
