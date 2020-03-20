using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmp.Models;
using WebApiEmp.Controllers;
using Microsoft.Extensions.Logging;

namespace WebApiEmp
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = "Server=(localdb)\\mssqllocaldb;Database=employees;Trusted_Connection=True;";


            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            services.AddDbContext<LogContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            services.AddMvc()
                .AddXmlDataContractSerializerFormatters()
                .AddMvcOptions(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/xml"));
                });

            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Caching", new CacheProfile()
                {
                    Duration = 300,
                    Location = ResponseCacheLocation.Any
                });
                options.CacheProfiles.Add("NoCaching", new CacheProfile()
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
            });

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
