using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static MVC_App.ProcessModel;
using MVC_App.Models;
using System;
using System.Net.Http.Headers;
using System.IO;
using System.Threading;

namespace MVC_App
{

    public class HomeController : Controller
    {
        ISiccarConnector _connector;
        ISiccarConfig _config;
        ISiccarHttpClient _siccarHttpClient;
        ISiccarStatusCache _statusCache;

        public HomeController(ISiccarConnector connector, ISiccarConfig config, ISiccarHttpClient httpClient, ISiccarStatusCache statusCache)
        {
            _connector = connector;
            _config = config;
            _siccarHttpClient = httpClient;
            _statusCache = statusCache;
        }

        [Authorize]
        public IActionResult Processes()
        {
            return View(BuildIndexModel());
        }

        [Authorize]
        public IActionResult Cards()
        {
            return View(BuildIndexModel());
        }

        public IActionResult Index()
        {
            return View();
        }

        private SiccarStatusCacheResponse BuildIndexModel()
        {

            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var isUserAddedToCache = _statusCache.HasUserBeenProcessed(userId);
            try
            {
                if (!isUserAddedToCache)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        if (_statusCache.HasUserBeenProcessed(userId))
                        {
                            break;
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                }
                var status = _statusCache.GetStatus(userId);
                return status;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Authorize]
        public IActionResult Confirmation()
        {
            return View();
        }

        [Authorize]
        public IActionResult Progress()
        {
            return View(BuildIndexModel());
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
            var attestationToken = await _siccarHttpClient.ExtendTokenClaims(_config.TokenEndpoint, idToken, _config.ExpectedClaims);
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessB, _config.ProcessBVersion, idToken, attestationToken);
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessC()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var attestationToken = await _siccarHttpClient.ExtendTokenAttestation(_config.TokenEndpoint, idToken, _config.ExpectedAttestations);
            var content = await _connector.GetStepNextOrStartProcess(_config.ProcessC, _config.ProcessCVersion, idToken, attestationToken);
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        public JObject AddImageToSubmission(string id, string fileName, string mimetype, IFormFile file)
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
            JObject value = new JObject();
            value.Add("Base64Data", bytes);
            value.Add("FileName", fileName);
            value.Add("MimeType", mimetype);

            JObject fileUpload = new JObject();
            fileUpload.Add("id", id);
            fileUpload.Add("value", value.ToString());
            return fileUpload;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAction(IFormCollection coll)
        {
            var stepComment = "";
            JArray fields = new JArray();
            var schemaId = string.Empty;
            foreach (var Key in coll.Keys)
            {
                if (Key != "previousStepId"
                    && Key != "__RequestVerificationToken"
                    && !Key.StartsWith("xxx"))
                {
                    JObject f = new JObject();
                    f.Add("id", Key);
                    f.Add("value", coll[Key].ToString());
                    fields.Add(f);
                }
                if (Key == "xxx-Process-Schema")
                {
                    schemaId = coll[Key].ToString();
                }
            }

            foreach (var file in coll.Files)
            {
                fields.Add(AddImageToSubmission(file.Name, file.FileName, file.ContentType, file));
            }


            dynamic post = new { stepComment, fields };
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            await _connector.SubmitStep(post, idToken, coll["previousStepId"]);
            _statusCache.AddUserToJustCompletedStepCache(userId, schemaId);
            _statusCache.UpdateProgressReportToReflectSubmission(userId);
            return RedirectToAction("Confirmation");
        }
    }
}
