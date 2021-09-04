using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleCrm.SqlDbServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IGreeter, ConfigurationGreeter>();
            services.AddSingleton<ICustomerData, SqlCustomerData>();

            services.AddDbContext<SimpleCrmDbContext>(opttions =>
            {
                opttions.UseSqlServer(Configuration.GetConnectionString("SimpleCrmConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                 app.UseDeveloperExceptionPage();
            }
            else
            {  
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Oops!")
                });
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default", 
                    "{controller=home}/{action=index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "contact",
                    pattern: "Contact/{phone}",
                    constraints: new { phone = "^\\d{3}-\\d{3}-\\d{4}$", },
                    defaults: new { controller = "Contact", action = "List", }
                 );
            });

            app.Run(ctx => ctx.Response.WriteAsync("Not found"));
        }
    }
}
