using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using Siccar.Connector.Connector;
using Siccar.Connector.Http;
using Siccar.CacheManager;
using Siccar.CacheManager.Models;
using Siccar.Connector.STS;
using Siccar.FormManager;
using MVC_App.Models;
using System.Collections.Generic;

namespace MVC_App
{

    public class HomeController : Controller
    {
        ISiccarConnector _connector;
        ISiccarConfig _config;
        ISiccarSTSClient _siccarStsClient;
        ISiccarStatusCache _statusCache;
        ISiccarEndpoints _siccarEndpoints;
        ISiccarFormManager _siccarFormManager;

        public HomeController(ISiccarConnector connector, ISiccarConfig config, ISiccarStatusCache statusCache, ISiccarSTSClient siccarStsClient, ISiccarEndpoints siccarEndpoints, ISiccarFormManager siccarFormManager)
        {
            _connector = connector;
            _config = config;
            _statusCache = statusCache;
            _siccarStsClient = siccarStsClient;
            _siccarEndpoints = siccarEndpoints;
            _siccarFormManager = siccarFormManager;
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

        private SiccarStatusCacheResponseViewModel BuildIndexModel()
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
                return new SiccarStatusCacheResponseViewModel(status);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Authorize]
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Progress()
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
            var content = await _connector.GetStepNextOrStartProcess(idToken, _config.ProcessA);
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessB()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var claimToken = await _siccarStsClient.ExtendTokenClaims(_siccarEndpoints.TokenEndpoint, idToken, _config.ExpectedClaims);
            var content = await _connector.GetStepNextOrStartProcess(idToken, _config.ProcessB, null, new List<string>() { claimToken });
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        public async Task<IActionResult> StartProcessC()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var attestationToken = await _siccarStsClient.ExtendTokenAttestation(_siccarEndpoints.TokenEndpoint, idToken, _config.ExpectedAttestations);
            var content = await _connector.GetStepNextOrStartProcess(idToken, _config.ProcessC, null, new List<string>() { attestationToken } );
            dynamic model = JsonConvert.DeserializeObject(content);
            ViewData["Payload"] = content;
            return View("Api", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAction(IFormCollection coll)
        {

            dynamic post = _siccarFormManager.BuildFormSubmissionModel(coll);
            var schemaId = _siccarFormManager.ReturnFieldFromSubmission(coll, "xxx-Process-Schema");

            var idToken = HttpContext.User.FindFirst("id_token").Value;
            var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            
            
            await _connector.SubmitStep(post, idToken, coll["previousStepId"]);
            _statusCache.AddUserToJustCompletedStepCache(userId, schemaId);
            _statusCache.UpdateProgressReportToReflectSubmission(userId);
            return RedirectToAction("Confirmation");
        }
    }
}
