using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.ComponentModel;
using static MVC_App.ProcessModel;
using System.Net.Http;

namespace MVC_App
{

    public class HomeController : Controller
    {
        ISiccarConnector _connector;
        ISiccarConfig _config;
        ISiccarHttpClient _siccarHttpClient;
        public HomeController(ISiccarConnector connector, ISiccarConfig config, ISiccarHttpClient httpClient)
        {
            _connector = connector;
            _config = config;
            _siccarHttpClient = httpClient;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Progress()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var resp = "[{\"schemaId\":\"0410b038-6d50-4568-ae29-6b21eace1581\",\"schemaInstanceId\":\"b76583b9-2737-416c-abc6-47fed57c8dd2\",\"schemaTitle\":\"Simple Two Step Private Wallet \",\"stepStatuses\":[{\"stepIndex\":0,\"stepTitle\":\"Single User Wallet \",\"completionTime\":1563192239},{\"stepIndex\":1,\"stepTitle\":\"Please authorise \",\"completionTime\":1563192281}]},{\"schemaId\":\"0c189e4f-e1d2-440c-867b-115d211b458d\",\"schemaInstanceId\":\"76bb0153-aeb8-4a2c-a87b-b5f58ea056ca\",\"schemaTitle\":\"Process A for Phase  3\",\"stepStatuses\":[{\"stepIndex\":0,\"stepTitle\":\"Application Form for Blue Badge\",\"completionTime\":1569230795},{\"stepIndex\":1,\"stepTitle\":\"Validate\",\"completionTime\":1569251755},{\"stepIndex\":2,\"stepTitle\":\"Authorise\",\"completionTime\":null}]},{\"schemaId\":\"0c189e4f-e1d2-440c-867b-115d211b458d\",\"schemaInstanceId\":\"dfe02c3a-8d64-4879-a9b5-c02f89cecd8b\",\"schemaTitle\":\"Process A for Phase  3\",\"stepStatuses\":[{\"stepIndex\":0,\"stepTitle\":\"Application Form for Blue Badge\",\"completionTime\":1569240435},{\"stepIndex\":1,\"stepTitle\":\"Validate\",\"completionTime\":1569251708},{\"stepIndex\":2,\"stepTitle\":\"Authorise\",\"completionTime\":null}]},{\"schemaId\":\"0c189e4f-e1d2-440c-867b-115d211b458d\",\"schemaInstanceId\":\"e98769ec-7cf5-4055-b1b0-4c22697808df\",\"schemaTitle\":\"Process A for Phase  3\",\"stepStatuses\":[{\"stepIndex\":0,\"stepTitle\":\"Application Form for Blue Badge\",\"completionTime\":1569243932},{\"stepIndex\":1,\"stepTitle\":\"Validate\",\"completionTime\":1569251668},{\"stepIndex\":2,\"stepTitle\":\"Authorise\",\"completionTime\":null}]},{\"schemaId\":\"0c189e4f-e1d2-440c-867b-115d211b458d\",\"schemaInstanceId\":\"a27a2f1f-66b2-4af2-b583-19684d5b34d4\",\"schemaTitle\":\"Process A for Phase  3\",\"stepStatuses\":[{\"stepIndex\":0,\"stepTitle\":\"Application Form for Blue Badge\",\"completionTime\":1569245952},{\"stepIndex\":1,\"stepTitle\":\"Validate\",\"completionTime\":1569246121},{\"stepIndex\":2,\"stepTitle\":\"Authorise\",\"completionTime\":null}]}]";
            var response = await _connector.GetProgressReport(idToken);
            
            JArray jsonObject = JArray.Parse(resp);
            var processSchemaList = jsonObject.ToObject<List<ProcessSchema>>();

            var list = new List<DataTable>();
            foreach (var v in processSchemaList)
            {
                DataTable dt = new DataTable();
                dt.TableName = v.schemaTitle;
                dt.Columns.Add("Current Step", typeof(int));
                dt.Columns.Add("Current Step title", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                
                DataTableHelper.Populate(dt, v.stepStatuses);

                list.Add(dt);
            }

            return View(list);
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> StartProcessA()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var attestationToken = await _siccarHttpClient.extendTokenAttestation(idToken);
            HttpContext.Session.SetString("attestationToken", attestationToken);
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessA, _config.ProcessAVersion, idToken);
            dynamic model = JsonConvert.DeserializeObject(content);
            
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessB()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessB, _config.ProcessBVersion, idToken);
            dynamic model = JsonConvert.DeserializeObject(content);


            ViewData["Payload"] = content;
            return View("Api", model);

        }

        [Authorize]
        public async Task<IActionResult> StartProcessC()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessC, _config.ProcessCVersion, idToken);
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAction(Dictionary<string, string> parameters)
        {
            var actorId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            JArray fields = new JArray();
            foreach (var pair in parameters)
            {
                if (pair.Key != "previousStepId" 
                    && pair.Key != "__RequestVerificationToken"
                    && !pair.Key.StartsWith("xxx"))
                {
                    JObject f = new JObject();
                    f.Add("id", pair.Key);
                    f.Add("value", pair.Value);
                    fields.Add(f);
                }
            }

            dynamic post = new { actorId, fields };

            var idToken = HttpContext.User.FindFirst("id_token").Value;
            await _connector.SubmitStep(post, idToken, parameters["previousStepId"]);

            return RedirectToAction("Index");

        }

    }
}
