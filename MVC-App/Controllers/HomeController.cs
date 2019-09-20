using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Siccar;
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
        public IActionResult Progress()
        {
            var idToken = HttpContext.User.FindFirst("id_token").Value;

            var response = _connector.GetProgressReport(idToken);
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
    }
}