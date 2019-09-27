using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public interface ISiccarHttpClient
    {
        Task<string> Get(string url, string idToken, bool ensureResponseIsValid = true);

        Task<string> Poll(string url, string idToken);


        Task<string> Post(string url, string idToken, string content, bool ensureResponseIsValid = true);
        Task<string> Post(string url, string idToken, string content, string token = null);
        Task<string> extendTokenAttestation(string token);
        Task<string> extendTokenClaims(string token);
    }
}