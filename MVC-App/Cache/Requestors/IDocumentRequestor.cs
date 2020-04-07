using System.Threading.Tasks;

namespace MVC_App.Cache.Requestors
{
    public interface IDocumentRequestor
    {
        Task<string> FetchDocument(string idToken, string transactionHash);
    }
}