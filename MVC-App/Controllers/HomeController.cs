using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MVC_App.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            return View("Views/Home/Progress.cshtml");
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Api()
        {
            string idToken = HttpContext.User.FindFirst("id_token").Value;

            ViewData["Payload"] = _connector.GetStepNextOrStartProcess("1", idToken);
            return View();
        }

    }
}

        
    

