using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Siccar.FormManager
{
    public class SiccarFormManager : ISiccarFormManager
    {
        private static string BASE64DATA = "Base64Data";
        private static string FILENAME = "FileName";
        private static string MIMETYPE = "MimeType";


        public string ReturnFieldFromSubmission(IFormCollection coll, string field)
        {
            return coll[field].ToString();
        }

        public dynamic BuildFormSubmissionModel(Dictionary<string, StringValues> elements, string stepComment = "")
        {
            return BuildFormSubmissionModel(BuildFormCollectionFromDictionary(elements), stepComment);
        }

        public dynamic BuildFormSubmissionModel(IFormCollection coll, string stepComment = "")
         {
            List<string> elementsNotToSubmit = new List<string>() { "previousStepId", "__RequestVerificationToken", "xxx" };
            JArray fields = new JArray();

            foreach (var Key in coll.Keys)
            {
                var matchFound = elementsNotToSubmit.Exists(x => Key.StartsWith(x));
                if (!matchFound)
                {
                    fields.Add(BuildJOBject(Key, coll[Key].ToString()));
                }
            }

            foreach (var file in coll.Files)
            {
                fields.Add(AddImageToSubmission(file.Name, file.FileName, file.ContentType, file));
            }


            dynamic post = new { stepComment, fields };
            return post;
        }

        private IFormCollection BuildFormCollectionFromDictionary(Dictionary<string, StringValues> elements)
        {
            return new FormCollection(elements);
        }

        private JObject AddImageToSubmission(string id, string fileName, string mimetype, IFormFile file)
        {
            return BuildJOBject(id, BuildFileJobject(fileName, mimetype, file).ToString());
        }

        private static JObject BuildFileJobject(string fileName, string mimetype, IFormFile file)
        {
            byte[] bytes;
            using (var stream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }
            }
            return new JObject
            {
                { BASE64DATA, bytes },
                { FILENAME, fileName },
                { MIMETYPE, mimetype }
            };
        }





        private JObject BuildJOBject(string key, string value)
        {
            return new JObject
            {
                { "id", key },
                { "value", value }
            };
        }


    }
}
