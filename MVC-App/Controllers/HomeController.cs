using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            string responseString = "";
            try
            {
                var scope = B2CConfig.ApiScopes.Split(' ');
                string signedInUserID = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                IConfidentialClientApplication cca =
                    ConfidentialClientApplicationBuilder.Create(B2CConfig.ClientId)
                    .WithRedirectUri(B2CConfig.RedirectUri)
                    .WithClientSecret(B2CConfig.ClientSecret)
                    .WithB2CAuthority(B2CConfig.Authority)
                    .Build();
                new MSALStaticCache(signedInUserID, HttpContext).EnablePersistence(cca.UserTokenCache);

                var accounts = await cca.GetAccountsAsync();
                AuthenticationResult result = await cca.AcquireTokenSilent(scope, accounts.FirstOrDefault())
                    .ExecuteAsync();

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, B2CConfig.ApiUrl);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        responseString = await response.Content.ReadAsStringAsync();
                        break;
                    case HttpStatusCode.Unauthorized:
                        responseString = $"Please sign in again. {response.ReasonPhrase}";
                        break;
                    default:
                        responseString = $"Error calling API. StatusCode=${response.StatusCode}";
                        break;
                }
            }

            catch (MsalUiRequiredException ex)
            {
                responseString = $"Session has expired. Please sign in again. {ex.Message}";
            }
            catch (Exception ex)
            {
                responseString = $"Error calling API: {ex.Message}";
            }

            ViewData["Payload"] = $"{responseString}";
            return View();
        }
    }
}

        
    

