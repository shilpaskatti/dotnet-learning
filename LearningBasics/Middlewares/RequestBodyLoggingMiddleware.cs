using Serilog;
using Serilog.Context;
using System.Text;
using System.Text.Json;

namespace LearningBasics.Middlewares
{
    public class RequestBodyLoggingMiddlewar(RequestDelegate requestDelegate)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Allow the request body to be read multiple times
            context.Request.EnableBuffering();

            string requestBody = string.Empty;

            // Leave stream open so the API controller can read it later
            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                // Reset the stream position to the beginning for downstream pipeline
                context.Request.Body.Position = 0;
            }
            object? structuredBody = null;

            if (!string.IsNullOrWhiteSpace(requestBody) &&
            context.Request.ContentType?.Contains("application/json") == true)
            {
                structuredBody = JsonSerializer.Deserialize<Dictionary<string, object?>>(requestBody);
            }

            using (LogContext.PushProperty("Body", structuredBody, destructureObjects: true))
            {
                Log.Information("Request payload {@PayloadBody}", structuredBody);

                await requestDelegate(context);
            }


           

        }
    }
}
