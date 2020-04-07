using System.Threading.Tasks;

namespace MVC_App.Cache.Requestors
{
    public interface ISiccarTransactionRequestor
    {
        Task<string> FetchTransaction(string idToken, string transactionHash);
    }
}