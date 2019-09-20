using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC_App
{

    public class HomeController : Controller
    {
        ISiccarConnector _connector;
        
        public HomeController(ISiccarConnector connector)
        {
            _connector = connector;
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
        public IActionResult Api()
        {
            string idToken = HttpContext.User.FindFirst("id_token").Value;

            ViewData["Payload"] = _connector.GetStepNextOrStartProcess("1", idToken);
            return View();
        }

    }
}

        
    

