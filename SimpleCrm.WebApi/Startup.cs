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
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SimpleCrm.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string SecretKey = "whywontyouwork"; //<-- NEW: make up a random key here
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
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

            var jwtOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                // optionally: allow configuration overide of ValidFor (defaults to 120 mins)
            });

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
            var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddGoogle(options =>
             {
                 options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                 options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
             })
             .AddMicrosoftAccount(options =>
             {
                 options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                 options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
             })
             .AddJwtBearer(configureOptions =>
             {
                 configureOptions.ClaimsIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                 configureOptions.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
                 configureOptions.SaveToken = true;
             });
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

                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseRouting();
            app.UseAuthentication();
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
