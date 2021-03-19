using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClfApi.Context;
using ClfApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace ClfApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowAllHeadersPolicy = "MyAllowAllHeadersPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowAllHeadersPolicy,
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:4200",
                                            "http://localhost:4200")
                                .AllowAnyHeader();
                    });
            });

            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<ClfDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClfDb")));

            services.AddScoped<ClfService>();

            services.AddSingleton<UtilService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowAllHeadersPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
