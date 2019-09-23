using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_App
{

    public class HomeController : Controller
    {
        ISiccarConnector _connector;
        ISiccarConfig _config;

        public HomeController(ISiccarConnector connector, ISiccarConfig config)
        {
            _connector = connector;
            _config = config;
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
            ViewData["Progress"] = response;
            return View("Views/Home/Progress.cshtml");
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
            ViewData["Payload"] = await _connector.GetStepNextOrStartProcess(_config.ProcessA, _config.ProcessAVersion, idToken);
            return View("Api");
        }

        [Authorize]
        public async Task<IActionResult> StartProcessB()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            ViewData["Payload"] = await _connector.GetStepNextOrStartProcess(_config.ProcessB, _config.ProcessBVersion, idToken);
            return View("Api");
        }

        [Authorize]
        public async Task<IActionResult> StartProcessC()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            ViewData["Payload"] = await _connector.GetStepNextOrStartProcess(_config.ProcessC, _config.ProcessCVersion, idToken);
            return View("Api");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAction(Dictionary<string, string> parameters)
        {
            var actorId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            JArray fields = new JArray();
            foreach (var pair in parameters)
            {
                if (pair.Key != "previousStepId" && pair.Key != "__RequestVerificationToken")
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
          

            return View("Api");

        }
    }
}

//{
//  "actorId": "string",
//  "fields": [
//    {
//      "id": "string",
//      "value": "string"
//    }
//  ]
//}