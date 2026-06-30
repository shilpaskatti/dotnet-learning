
using FluentValidation;
using LearningBasics.Data;
using LearningBasics.DTOs.Request;
using LearningBasics.Filters;
using LearningBasics.Middlewares;
using LearningBasics.Repository.Users;
using LearningBasics.Services;
using LearningBasics.Validations.User;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

try
{
    Log.Information("Starting Service...");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
       .ReadFrom.Configuration(builder.Configuration) // <-- Reads your File & Seq setup
       .Destructure.ByTransforming<LoginUserRequest>((request) =>
       {
           return new { UserName = request.UserName, Password = "****Redacted****" };
       })
       .ReadFrom.Services(services) // <-- Connects internal .NET services
   );


    // Add services to the container.

    builder.Services.AddProblemDetails();
    builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

    builder.Services.AddControllers((options) =>
    {
        options.Filters.Add<RequestLoggingFilter>();
    });
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<AppDbContext>(
                    options => options.UseSqlServer(
                       connectionString));


    #region Dependency Injection
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    #endregion

    var app = builder.Build();
    app.UseMiddleware<CorrelationIdMiddleware>();

    app.UseMiddleware<ExceptionMiddleware>();
    //app.UseExceptionHandler();


    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);

            if (httpContext.Items.TryGetValue(
                    RequestLoggingFilter.RequestPayloadKey,
                    out var payload))
            {
                diagnosticContext.Set(
                    "RequestPayload",
                    payload,
                    destructureObjects: true);
            }
        };
    });
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        //app.UseSwaggerUI(options =>
        //{
        //    options.SwaggerEndpoint("/openapi/v1.json", "v1");
        //});
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Issues detected in server");
}
finally
{
    Log.CloseAndFlush();
}


