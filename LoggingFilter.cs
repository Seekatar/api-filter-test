using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.Json;

internal class LoggingFilter :  IAuthorizationFilter,   // runs first
                                IResourceFilter,        // runs second & sixth
                                IActionFilter,          // runs third & fourth
                                IResultFilter,          // runs fifth
                                // IEndpointFilter,        // runs only if action method runs ok
                                IExceptionFilter        // runs if exception is thrown
{
    private readonly ILogger<LoggingFilter> _logger;
    List<IDisposable> _scopes = new();
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", System.Reflection.MethodBase.GetCurrentMethod()!.Name } }));
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
        _logger.LogDebug(JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true }));
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", System.Reflection.MethodBase.GetCurrentMethod()!.Name } }));
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnException(ExceptionContext context)
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", System.Reflection.MethodBase.GetCurrentMethod()!.Name } }));
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", System.Reflection.MethodBase.GetCurrentMethod()!.Name } }));
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", System.Reflection.MethodBase.GetCurrentMethod()!.Name } }));
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);
    }
}
