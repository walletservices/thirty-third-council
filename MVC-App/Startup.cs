using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using MVC_App.Siccar;
using Siccar.CacheManager;
using Siccar.CacheManager.Caches;
using Siccar.CacheManager.ModelManagers;
using Siccar.CacheManager.Requestors;
using Siccar.Connector.Connector;
using Siccar.Connector.Http;
using Siccar.Connector.STS;
using Siccar.FormManager;

namespace MVC_App
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ConfigureSiccarServices(services);

            var key = Encoding.ASCII.GetBytes("Authentication:AzureAdB2C:ClientSecret");
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            })
             .AddAzureAdB2C(options =>
             {
                 Configuration.Bind("Authentication:AzureAdB2C", options);
             },
            services.BuildServiceProvider().GetService<ISiccarStatusCache>()
            )
            .AddJwtBearer().AddCookie();
            services.AddNodeServices();


            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;


            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private void ConfigureSiccarServices(IServiceCollection services)
        {
            var siccarconfig = new SiccarConfig();
            Configuration.Bind("SiccarConfig", siccarconfig);


            services.AddSingleton<IMemoryCache, MemoryCache>();

            var siccarendpoints = new SiccarEndpoints();
            Configuration.Bind("SiccarEndpoints", siccarendpoints);

            var client = new SiccarHttpClient(new System.Net.Http.HttpClient());

            services.AddSingleton<ISiccarConfig>(siccarconfig);
            services.AddSingleton<ISiccarEndpoints>(siccarendpoints);

            services.AddSingleton<ISiccarConnector>((container) =>
            {
                return new SiccarConnector(siccarendpoints, client, container.GetRequiredService<ILogger<SiccarConnector>>());
            });


            services.AddSingleton<ISiccarHttpClient>(client);


            services.AddSingleton<ISiccarSTSClient>(new SiccarSTSClient(siccarconfig.SiccarSTSClientId, new System.Net.Http.HttpClient()));

            services.AddSingleton<ISiccarStatusCache, SiccarStatusCache>();
            services.AddSingleton<IHostedService, TriggerCacheService>();
            services.AddSingleton<ISiccarTransactionRequestor, SiccarTransactionRequestor>();
            services.AddSingleton<IProgressReportRequestor, ProgressReportRequestor>();
            services.AddSingleton<IDocumentRequestor, DocumentRequestor>();

            services.AddSingleton<IUserCache, UserCache>();
            services.AddSingleton<IUserJustCompletedStepCache, UserJustCompletedStepCache>();
            services.AddSingleton<ISiccarTransactionCache, SiccarTransactionCache>();
            services.AddSingleton<IProgressReportCache, ProgressReportCache>();
            services.AddSingleton<IProcessSchemaCache, ProcessSchemaCache>();
            services.AddSingleton<ISiccarStatusCacheResponseManager, SiccarStatusCacheResponseManager>();

            services.AddSingleton<IProgressReportModelManager, ProgressReportModelManager>();
            services.AddSingleton<ISiccarTransactionManager, SiccarTransactionManager>();

            services.AddSingleton<ISiccarFormManager, SiccarFormManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:5001/",
                                    "https://localhost:8691/",
                                    "https://localhost:5001/");
            });
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "progress",
                    template: "{Controller=Home}/{action=Progress}");
            });
        }
    }
}
