using Serilog.Context;

namespace LearningBasics.Middlewares
{
    public class CorrelationIdMiddleware(RequestDelegate requestDelegate)
    {
        private const string CorrelationHeader = "CorrelationId";
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Response.Headers[CorrelationHeader] = correlationId;

            using (LogContext.PushProperty(CorrelationHeader, correlationId))
            {
                await requestDelegate(context);
            }

        }
    }
}
