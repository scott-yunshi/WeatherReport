using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrentWeather.Common.ValidationHelper
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class ApiKeyAuth : Attribute,IAsyncActionFilter
    {
        private const string ApiKeyName = "WeatherReportAPIAuthKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var keyInRequest))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeys = config.GetSection(ApiKeyName).Value.Split(',');

            if(!apiKeys.Contains(keyInRequest.ToString()))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
