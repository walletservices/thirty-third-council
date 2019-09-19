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
        B2CConfig B2CConfig;
        ISiccarConnector _connector;
        
        public HomeController(IOptions<B2CConfig> b2cConfig)
        {
            B2CConfig = b2cConfig.Value;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
       
            return View();
        }
        [Authorize]
        public IActionResult Progress()
        {
            return View("Views/Home/Progress.cshtml");
        }
        [Authorize]
        public async Task<IActionResult> Process()
        {
            return View();
        }
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Api()
        {
            string signedInUserID = HttpContext.User.FindFirst("id_token").Value;
            ViewData["Payload"] = $"{signedInUserID}";
            return View();
        }

        //public string getToken()
        //{
        //    AuthenticationResult result = null;
        //    try
        //    {
        //        result = await app.
        //    }
        //}
       
    }
}

        
    

