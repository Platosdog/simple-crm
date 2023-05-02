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
using NSwag.Generation.Processors.Security;
using NSwag;
using NSwag.AspNetCore;
using System.Collections.Generic;

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
            services.AddSwaggerDocument(config =>
            {
                config.DocumentName = "v1.0";
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1.0";
                };
                config.ApiGroupNames = new[] { "1.0" };
            });
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddSpaStaticFiles((config =>
            {
                config.RootPath = Configuration["SpaRoot"];
            }));
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
            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "SimpleCrm";
                options.Version = "1.0";
                options.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token", new List<string>(),
                new OpenApiSecurityScheme
                {
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Description = "Type into the textbox: `Bearer {your_JWT_token}`. You can get a JWT from endpoints: '/auth/register' or '/auth/login'"
                }));
                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
            });

            services.AddDefaultIdentity<CrmUser>()
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<CrmIdentityDbContext>();
                services.AddControllersWithViews();
                services.AddRazorPages();
                services.AddScoped<ICustomerData, SqlCustomerData>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)],
                    ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)],
                    AppName = "Simple CRM",
                    Realm = "Nexul Academy"
                };
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
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
   
