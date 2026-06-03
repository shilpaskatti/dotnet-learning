using LearningBasics.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace LearningBasics.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {

            logger.LogInformation($"Processing request {context.Request.Method} {context.Request.Path}");

            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    AppException appEx => (int)appEx.StatusCode,
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Status = context.Response.StatusCode,
                    Instance = context.Request.Path,
                    Detail = ex.StackTrace
                };

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;
                problemDetails.Extensions["timeStamp"] = DateTime.UtcNow;
                problemDetails.Extensions["errors"] = ex switch
                {
                    ValidationErrorException validationError => validationError.errors,
                    _ => null
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }


        }

    }
}
