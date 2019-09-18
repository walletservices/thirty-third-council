using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using MVC_App.Models;
using Microsoft.AspNetCore.Http;

namespace MVC_App
{
    public static class B2CAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder)
            => builder.AddAzureAdB2C(_ =>
            {
            });

        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder,
            Action<B2CConfig> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, OpenIdConnectOptionsSetup>();
            builder.AddOpenIdConnect();
            return builder;
        }

        public class OpenIdConnectOptionsSetup : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            public OpenIdConnectOptionsSetup(IOptions<B2CConfig> b2cOptions)
            {
                B2CConfig = b2cOptions.Value;
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
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived
                };
            }

            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }
            public async Task<int> OnRedirectToIdentityProvider(RedirectContext context)
            {
                var defaultPolicy = B2CConfig.DefaultPolicy;
                if (context.Properties.Items.TryGetValue(B2CConfig.PolicyAuthenticationProperty, out var policy)
                    && !policy.Equals(defaultPolicy))
                {
                    context.ProtocolMessage.Scope = OpenIdConnectScope.OpenIdProfile;
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.IdToken;
                    context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.ToLower().Replace(defaultPolicy.ToLower(), policy.ToLower());
                    context.Properties.Items.Remove(B2CConfig.PolicyAuthenticationProperty);

                }
                else if (!string.IsNullOrEmpty(B2CConfig.ApiUrl))
                {
                    context.ProtocolMessage.Scope += $" offline access {B2CConfig.ApiScopes}";
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    context.HttpContext.Session.SetString("hello", "world");


                }



                return await Task.FromResult(0);
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
            public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
            {
                // Use MSAL to swap the code for an access token
                // Extract the code from the response notification
                var code = context.ProtocolMessage.Code;

                

                string signedInUserID = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                string country = context.Principal.FindFirst(ClaimTypes.Country).Value;
                string address = context.Principal.FindFirst(ClaimTypes.StreetAddress).Value;
                string age = context.Principal.FindFirst(ClaimTypes.DateOfBirth).Value;
                string email = context.Principal.FindFirst(ClaimTypes.Email).Value;

                
                
                IConfidentialClientApplication cca = ConfidentialClientApplicationBuilder.Create(B2CConfig.ClientId)
                    .WithB2CAuthority(B2CConfig.Authority)
                    .WithRedirectUri(B2CConfig.RedirectUri)
                    .WithClientSecret(B2CConfig.ClientSecret)
                    .Build();
                new MSALStaticCache(signedInUserID, context.HttpContext).EnablePersistence(cca.UserTokenCache);
                try
                {
                    AuthenticationResult result = await cca.AcquireTokenByAuthorizationCode(B2CConfig.ApiScopes.Split(' '), code)
                        .ExecuteAsync();

                    context.HttpContext.Session.SetString("id_token", result.AccessToken);
                    context.HandleCodeRedemption(result.AccessToken, result.IdToken);


                }
                catch (Exception ex)
                {
                    //TODO: Handle
                    throw;
                }

            }


        }
    }
}


