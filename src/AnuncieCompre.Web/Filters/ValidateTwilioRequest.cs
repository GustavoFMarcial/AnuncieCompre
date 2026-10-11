using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Twilio.Security;

namespace AnuncieCompre.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateTwilioRequestAttribute : TypeFilterAttribute
    {
        public ValidateTwilioRequestAttribute() : base(typeof(ValidateTwilioRequestFilter))
        {
        }
    }

    internal class ValidateTwilioRequestFilter : IAsyncActionFilter
    {
        private readonly bool _isTestEnvironment;
        private readonly string _authToken;
        private readonly RequestValidator _requestValidator;

        public ValidateTwilioRequestFilter(IConfiguration configuration)
        {
            _isTestEnvironment = configuration.GetValue<bool>("IsTestEnvironment");
            _authToken = configuration["Twilio:AuthToken"] ?? throw new Exception("'Twilio:AuthToken' not configured.");
            _requestValidator = new RequestValidator(_authToken);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;

            if (_isTestEnvironment)
            {
                await next();
                return;
            }

            var requestUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            Dictionary<string, string> parameters = null!;

            if (request.HasFormContentType)
            {
                var form = await request.ReadFormAsync(httpContext.RequestAborted).ConfigureAwait(false);
                parameters = form.ToDictionary(p => p.Key, p => p.Value.ToString());
            }

            var signature = request.Headers["X-Twilio-Signature"];
            var isValid = _requestValidator.Validate(requestUrl, parameters, signature);

            if (!isValid)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await next();
        }
    }
}