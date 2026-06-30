using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;

namespace LearningBasics.Filters
{
    public class RequestLoggingFilter() : IActionFilter
    {
        public const string RequestPayloadKey = "RequestPayload";

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var arguments = context.ActionArguments;
            context.HttpContext.Items[RequestPayloadKey] = arguments;
        }
    }
}
