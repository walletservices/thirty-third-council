using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using MVC_App.Siccar;
using System.Linq;
using Siccar.CacheManager;

namespace MVC_App
{
    public static class B2CAuthenticationBuilderExtensions
    {
        private static ISiccarStatusCache _cache;

        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder)
            => builder.AddAzureAdB2C(_ =>
            {
            }, _cache);

        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder,
            Action<B2CConfig> configureOptions, ISiccarStatusCache cache)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<ISiccarStatusCache>(cache);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, OpenIdConnectOptionsSetup>();
            builder.AddOpenIdConnect();
            return builder;
        }

        public class OpenIdConnectOptionsSetup : IConfigureNamedOptions<OpenIdConnectOptions>
        {

            private string tempIdToken;
            private ISiccarStatusCache _cache;


            public OpenIdConnectOptionsSetup(IOptions<B2CConfig> b2cOptions, ISiccarStatusCache cache)
            {
                B2CConfig = b2cOptions.Value;
                _cache = cache;
            }

            public B2CConfig B2CConfig { get; set; }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                options.ClientId = B2CConfig.ClientId;
                options.Authority = B2CConfig.Authority;
                options.UseTokenLifetime = true;
                options.TokenValidationParameters = new TokenValidationParameters() { NameClaimType = "name" };


                options.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    OnRemoteFailure = OnRemoteFailure,
                    OnTicketReceived = OnTokenReceived,
                    OnMessageReceived = OnMessageReceived,
                };

                // Usually this is the event that we use but with the config of the Open id connect config it doesnt work
                //options.Events = new OpenIdConnectEvents()
                //{
                //    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                //};
            }


            public async Task<int> OnMessageReceived(MessageReceivedContext context)
            {
                if (context != null && context.ProtocolMessage != null && context.ProtocolMessage.IdToken != null)
                {
                    tempIdToken = context.ProtocolMessage.IdToken;
                }

                return await Task.FromResult(0);

            }
            public async Task<int> OnTokenReceived(TicketReceivedContext context)
            {
                var identity = context.Principal.Identity as ClaimsIdentity;
                identity.AddClaim(new Claim("id_token", tempIdToken));
                var name = identity.Claims.ToList().Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                //Do not wait for this , the cache will get upated in the background
                _cache.RefreshAndDontWait(name.Value, tempIdToken);
                return await Task.FromResult(0);

            }


            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            public Task OnRedirectToIdentityProvider(RedirectContext context)
            {
                var defaultPolicy = B2CConfig.DefaultPolicy;
                if (context.Properties.Items.TryGetValue(B2CConfig.PolicyAuthenticationProperty, out var policy) &&
                    !policy.Equals(defaultPolicy))
                {
                    context.ProtocolMessage.Scope = OpenIdConnectScope.OpenIdProfile;
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.IdToken;
                    context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.ToLower().Replace(defaultPolicy.ToLower(), policy.ToLower());
                    context.Properties.Items.Remove(B2CConfig.PolicyAuthenticationProperty);
                }
                else if (!string.IsNullOrEmpty(B2CConfig.ApiUrl))
                {
                    context.ProtocolMessage.Scope += $" offline_access {B2CConfig.ApiScopes}";
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                }
                return Task.FromResult(0);
            }


            public Task OnRemoteFailure(RemoteFailureContext context)
            {
                context.HandleResponse();
                // Handle the error code that Azure AD B2C throws when trying to reset a password from the login page 
                // because password reset is not supported by a "sign-up or sign-in policy"
                if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("AADB2C90118"))
                {
                    // If the user clicked the reset password link, redirect to the reset password route
                    context.Response.Redirect("/Session/ResetPassword");
                }
                else if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("access_denied"))
                {
                    context.Response.Redirect("/");
                }
                else
                {
                    context.Response.Redirect("/Home/Error?message=" + Uri.EscapeDataString(context.Failure.Message));
                }
                return Task.FromResult(0);
            }
            
        }
    }
}


