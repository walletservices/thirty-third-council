using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.Connector.Http
{
    public interface ISiccarHttpClient
    {
        Task<string> Get(string url, string idToken, bool ensureResponseIsValid = true);

        Task<string> Poll(string url, string idToken);


        Task<string> Post(string url, string idToken, string content, bool ensureResponseIsValid = true);
        Task<string> Post(string url, string idToken, string content, List<string> tokens = null);

        Task<FileContentResult> GetDocument(string url, string idToken, bool ensureResponseIsValid = true);

    }
}