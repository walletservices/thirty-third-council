using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using MVC_App.Models;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MVC_App
{

    public static class GraphClient
    {
        public static async Task<Microsoft.Graph.User> GetUserDetailsAsync(HttpContext httpContext)
        {
            B2CConfig B2cConfig = new B2CConfig();
            var scope = B2cConfig.ApiScopes.Split(' ');
            string signedInUserID = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            IConfidentialClientApplication cca =
            ConfidentialClientApplicationBuilder.Create(B2cConfig.ClientId)
                .WithRedirectUri(B2cConfig.RedirectUri)
                .WithClientSecret(B2cConfig.ClientSecret)
                .WithB2CAuthority(B2cConfig.Authority + "/" + B2cConfig.SignUpSignInPolicyId)
                .Build();
            
            new MSALStaticCache(signedInUserID, httpContext).EnablePersistence(cca.UserTokenCache);
            var accounts = await cca.GetAccountsAsync();
            AuthenticationResult result = await cca.AcquireTokenSilent(scope, accounts.FirstOrDefault())
                .ExecuteAsync();
            var graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));

            return await graphClient.Me.Request().GetAsync();
        }
    }
}
