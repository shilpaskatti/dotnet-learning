using LearningBasics.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningBasics.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, $"An unhandled exception occurred while processing the request. TraceId: {httpContext.TraceIdentifier}");
            var (statusCode, title) = GetStatusCode(exception);
            var problemDetails = exception switch
            {
                ValidationErrorException validationError => new HttpValidationProblemDetails(validationError.errors)
                {
                    Title = title,
                    Status = statusCode,
                    Detail = exception.StackTrace,
                    Instance = httpContext.Request.Path
                },
                _ => new ProblemDetails
                {
                    Title = title,
                    Status = statusCode,
                    Detail = exception.StackTrace,
                    Instance = httpContext.Request.Path
                }
            };
               
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            

            httpContext.Response.StatusCode = statusCode;
           return await problemDetailsService.TryWriteAsync
                (new ProblemDetailsContext{   
                    ProblemDetails = problemDetails, 
                    HttpContext = httpContext
                });

        }


        private (int,string) GetStatusCode(Exception exception)
            => exception switch
            {
                AppException appEx => ((int)appEx.StatusCode, appEx.Message),
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,"Unauthorized access"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

        
    }
}
