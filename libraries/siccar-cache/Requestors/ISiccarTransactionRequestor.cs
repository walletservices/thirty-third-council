using System.Threading.Tasks;

namespace Siccar.CacheManager.Requestors
{
    public interface ISiccarTransactionRequestor
    {
        Task<string> FetchTransaction(string idToken, string transactionHash);
    }
}