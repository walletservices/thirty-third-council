﻿
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MVC_App.Cache.Caches;
using MVC_App.Siccar;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC_App.Controllers
{
    [EnableCors("MyPolicy")]
    public class SessionController : Controller
    {
        private ISiccarStatusCache _statusCache;

        public SessionController(IOptions<B2CConfig> b2cOptions, ISiccarStatusCache statusCache)
        {
            B2CConfig = b2cOptions.Value;
            _statusCache = statusCache;
        }
        public B2CConfig B2CConfig { get; set; }

        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");

            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public IActionResult SignOut()
        {
            var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            _statusCache.RemoveUser(userId);

            var callbackUrl = Url.Action(nameof(SignedOut), "Session", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }



    }
}
