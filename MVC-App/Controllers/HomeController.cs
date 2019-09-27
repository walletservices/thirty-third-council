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
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

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
            var response = await _connector.GetProgressReport(idToken);
            
            JArray jsonObject = JArray.Parse(response);
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
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessA, _config.ProcessAVersion, idToken);
            dynamic model = JsonConvert.DeserializeObject(content);
            
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessB()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var attestationToken = await _siccarHttpClient.extendTokenClaims(idToken);
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessB, _config.ProcessBVersion, idToken, attestationToken);
            dynamic model = JsonConvert.DeserializeObject(content);

            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessC()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var attestationToken = await _siccarHttpClient.extendTokenAttestation(idToken);
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessC, _config.ProcessCVersion, idToken, attestationToken);
            dynamic model = JsonConvert.DeserializeObject(content);

            ViewData["Payload"] = content;
            return View("Api", model);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAction(Dictionary<string, string> parameters)
        {
            var stepComment = "";

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

            dynamic post = new { stepComment, fields };

            var idToken = HttpContext.User.FindFirst("id_token").Value;
            await _connector.SubmitStep(post, idToken, parameters["previousStepId"]);

            return RedirectToAction("Index");

        }

    }
}
