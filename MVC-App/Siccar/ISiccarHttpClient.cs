using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public interface ISiccarHttpClient
    {
        Task<string> Get(string url, string idToken, bool ensureResponseIsValid = true);

        Task<string> Poll(string url, string idToken);


        Task<string> Post(string url, string idToken, string content, bool ensureResponseIsValid = true);
        Task<string> Post(string url, string idToken, string content, string token = null);
        Task<string> ExtendTokenAttestation(string url, string token, string attestations);
        Task<string> ExtendTokenClaims(string url, string token, string claims);
        Task<FileContentResult> GetDocument(string url, string idToken, bool ensureResponseIsValid = true);

    }
}