using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;

using System;
using System.Text;
using System.Text.Json;
using Serilog;

using Lightning.Core.Filters;

namespace Lightning.WebApi
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
            services.AddControllers(options => 
                {
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .ConfigureApiBehaviorOptions(options => 
                {
                    //options.SuppressModelStateInvalidFilter = true;
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{
                    Version = "v1",
                    Title = "Lightning Web Api"
                });

                options.IncludeXmlComments(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "Lightning.WebApi.xml"));
                options.IncludeXmlComments(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "Lightning.Application.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
