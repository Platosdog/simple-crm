using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCrm.SqlDbServices;
using System;
using SimpleCrm.WebApi.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Mvc;

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
            services.AddControllersWithViews();
            services.AddControllers();
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

            var tokenValidationPrms = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {   //tells ASP.Net Identity the application is using JWT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {   //tells ASP.Net to look for Bearer authentication with these options
                configureOptions.ClaimsIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationPrms;
                configureOptions.SaveToken = true; // allows token access in controller
            });

            var identityBuilder = services.AddIdentityCore<CrmUser>(o =>
            {
                //TODO: you may override any default password rules here.
            });
            identityBuilder = new IdentityBuilder(identityBuilder.UserType,
              typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<CrmIdentityDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<IdentityRole>>();
            identityBuilder.AddRoleManager<RoleManager<IdentityRole>>();
            identityBuilder.AddSignInManager<SignInManager<CrmUser>>();
            identityBuilder.AddDefaultTokenProviders();

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(
                  Constants.JwtClaimIdentifiers.Rol,
                  Constants.JwtClaims.ApiAccess
                ));

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
                         IssuerSigningKey = _signingKey,
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                     configureOptions.SaveToken = true;
                 });
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
   
