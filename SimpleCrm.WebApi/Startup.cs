using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleCrm.SqlDbServices;
using System;
using SimpleCrm.WebApi.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace SimpleCrm.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SimpleCrmDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SimpleCrmConnection")));

            services.AddDbContext<CrmIdentityDbContext>(options =>
                 options.UseSqlServer(
                    Configuration.GetConnectionString("SimpleCrmConnection")));

            services.AddDefaultIdentity<CrmUser>()
              .AddDefaultUI()
              .AddEntityFrameworkStores<CrmIdentityDbContext>();

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddScoped<ICustomerData, SqlCustomerData>();

            services.AddSpaStaticFiles(config => 
            {
                config.RootPath = Configuration["SpaRoot"];
            });
            var googleOptions = Configuration.GetSection(nameof(GoogleAuthSettings));

            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddGoogle(options =>  
              {
                    options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                    options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
                });
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
                 .AddMicrosoftIdentityUI();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                appBuilder => appBuilder.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "../simple-crm-cli";
                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                    spa.Options.StartupTimeout = new TimeSpan(0, 0, 300); //300 seconds
                              spa.UseAngularCliServer(npmScript: "start");
                  
              }));
        }
    }
}
