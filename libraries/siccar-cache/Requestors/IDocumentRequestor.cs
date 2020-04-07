using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
{
    public interface IDocumentRequestor
    {
        Task<string> FetchDocument(string idToken, string transactionHash);
    }
}