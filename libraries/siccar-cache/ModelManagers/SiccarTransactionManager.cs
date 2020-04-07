using Newtonsoft.Json;
using Siccar.CacheManager.Requestors;
using Siccar.Shallow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siccar.CacheManager.ModelManagers
{
    public class SiccarTransactionManager : ISiccarTransactionManager
    {
        private IDocumentRequestor _documentRequestor;

        public SiccarTransactionManager(IDocumentRequestor requestor)
        {
            _documentRequestor = requestor;
        }


        public async Task<List<string>> GetTransactionIdsFromProgressReport(HashSet<ProcessSchema> progressUpdate)
        {
            var list = new List<string>();
            foreach (var schema in progressUpdate)
            {
                var steps = schema.stepStatuses;
                if (steps.Count > 0)
                {
                    if (steps[0].stepTransactionId != null)
                    {
                        list.Add(steps[0].stepTransactionId);
                    }
                    if (steps[steps.Count - 1].stepTransactionId != null)
                    {
                        list.Add(steps[steps.Count - 1].stepTransactionId);
                    }
                }
            }
            return list;
        }


        public async Task<SiccarTransaction> BuildSingleTransactionView(string txId, string transactionContents, string idToken)
        {
            var list = new List<KeyValuePair<string, string>>();

            try
            {
                dynamic content = JsonConvert.DeserializeObject<dynamic>(transactionContents);
                dynamic privateArr = content.payload.@private;
                foreach (dynamic priv in privateArr)
                {
                    try
                    {
                        dynamic fields = priv.fields;
                        list.AddRange(await PopulateCacheWithFielddata(fields, idToken));

                    }
                    catch (Exception)
                    {
                        // cannot read this - so move on 
                    }
                }
                dynamic pfields = content.payload.@public.fields;
                list.AddRange(await PopulateCacheWithFielddata(pfields, idToken));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new SiccarTransaction() { TransactionId = txId, Attributes = list.ToHashSet() };
        }



        private async Task<List<KeyValuePair<string, string>>> PopulateCacheWithFielddata(dynamic fields, string idToken)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (dynamic field in fields)
            {
                dynamic label = field.label[0].display;
                list.Add(new KeyValuePair<string, string>(label.ToString(), await BuildFieldView(field.type.ToString(), field.value.ToString(), idToken)));
            }
            return list;
        }



        private async Task<string> BuildFieldView(string type, string value, string idToken)
        {
            if (type == "UploadDocument")
            {
                try
                {
                    dynamic jobject = JsonConvert.DeserializeObject<dynamic>(value);
                    var sourceTxId = jobject.TransactionId;
                    var response = await _documentRequestor.FetchDocument(idToken, sourceTxId.ToString());
                    return $"data:image;base64,{response}";
                }
                catch (Exception)
                {
                    return "Unable to fetch document";
                }
            }
            if (type == "Date")
            {
                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Int64.Parse(value));
                var dateTime = dateTimeOffset.UtcDateTime;
                return dateTime.ToString("dd/MM/yyyy");
            }
            return value;
        }
    }
}
