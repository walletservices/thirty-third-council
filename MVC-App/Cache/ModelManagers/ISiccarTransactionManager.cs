using System.Collections.Generic;
using System.Threading.Tasks;
using MVC_App.Models;

namespace MVC_App.Cache.V2
{
    public interface ISiccarTransactionManager
    {
        Task<SiccarTransaction> BuildSingleTransactionView(string tx, string siccarTransactionContents, string idToken);
        Task<List<string>> GetTransactionIdsFromProgressReport(HashSet<ProcessModel.ProcessSchema> progressUpdate);
    }
}