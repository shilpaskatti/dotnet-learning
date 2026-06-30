using LearningBasics.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LearningBasics.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {

            //logger.LogInformation($"Processing request {context.Request.Method} {context.Request.Path}");

            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var statusCode = ex switch
                {
                    AppException appEx => (int)appEx.StatusCode,
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;
                
                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Instance = context.Request.Path,
                    Title = statusCode switch
                    {
                        StatusCodes.Status400BadRequest => "Bad request.",
                        StatusCodes.Status404NotFound => "Resource not found.",
                        StatusCodes.Status409Conflict => "Conflict.",
                        _ => "An unexpected error occurred."
                    },
                    Detail = ex switch
                    {
                        ValidationErrorException => "One or more validation errors occurred.",
                        AppException appEx => appEx.Message,
                        ArgumentException => ex.Message,
                        _ => "An unexpected error occurred. Please try again later."
                    }
                };

                problemDetails.Extensions["correlationId"] = context.Response.Headers["CorrelationId"];
                problemDetails.Extensions["traceId"] = context.TraceIdentifier;
                problemDetails.Extensions["timeStamp"] = DateTime.UtcNow;
                problemDetails.Extensions["errors"] = ex switch
                {
                    ValidationErrorException validationError => validationError.errors,
                    _ => null
                };

                //logger.LogError(ex, $"An unhandled exception occurred while processing the request. TraceId: {context.TraceIdentifier}");
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}