using AspNetCoreRateLimit;
using CurrentWeather.Models.ApiConfigModels;
using CurrentWeather.Models.ViewModels;
using CurrentWeather.Service.Contracts;
using CurrentWeather.Service.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CurrentWeather
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<ClientRateLimitOptions>(_configuration.GetSection("ClientRateLimiting"));
            services.AddInMemoryRateLimiting();
            services.AddControllers();
            services.Configure<WeatherConfig>(_configuration.GetSection("WeatherConfig"));
            services.AddHttpClient<IOpenWeatherMapService, OpenWeatherMapService>();
            services.AddSwaggerGen();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());   

            app.UseClientRateLimiting();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API version 1.0"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Exception Middleware handler
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new ErrorModels { StatusCode = context.Response.StatusCode,  Message = exception.Message };
                await context.Response.WriteAsync(response.ToString());
            }));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
