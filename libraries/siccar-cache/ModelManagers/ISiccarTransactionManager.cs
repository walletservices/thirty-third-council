using Siccar.Shallow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siccar.CacheManager.ModelManagers
{
    public interface ISiccarTransactionManager
    {
        Task<SiccarTransaction> BuildSingleTransactionView(string tx, string siccarTransactionContents, string idToken);
        Task<List<string>> GetTransactionIdsFromProgressReport(HashSet<ProcessSchema> progressUpdate);
    }
}