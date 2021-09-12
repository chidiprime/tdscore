using AutoWrapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.Interfaces;
using TDS.Data;
using TDS.Service.Implementation;

namespace TDS.Api
{
    public class Startup
    {
        public Startup(IHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<TDSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ILog, Log>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAPILog, APILog>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IBackgroundService, BackgroundJobService>();
            services.AddTransient<IRestClient, RestClient>();
            services.Configure<Settings>(Configuration.GetSection("Settings"));
            services.AddHangfire(options => options.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddSwaggerGen(c =>
            {
             
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TDS",
                    Description = "Transaction Distributed Service",
                    Contact = new OpenApiContact
                    {
                        Name = "Securrency",
                        Email = string.Empty
                       
                    }
                   
                });

              
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var cronTime=Convert.ToInt32(Configuration.GetValue<string>("Settings:CronExpression"));
            RecurringJob.AddOrUpdate<IBackgroundService>(x =>x.ActivateWalletsAsync(), Cron.MinuteInterval(cronTime));
            RecurringJob.AddOrUpdate<IBackgroundService>(x => x.SaveLedgersAsync(), Cron.MinuteInterval(cronTime));
            app.UseApiResponseAndExceptionWrapper<MapResponseObject>(
               new AutoWrapperOptions
               {
                   IsApiOnly = false
               });
            app.UseSwagger(c => c.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seccurency API");
            });
           
            app.UseRouting();
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
            app.UseAuthorization();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
