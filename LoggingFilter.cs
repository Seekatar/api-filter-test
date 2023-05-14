using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class LoggingFilter :  LoggingFilterBase,
                                IAuthorizationFilter,   // runs first
                                IResourceFilter,        // runs second & sixth
                                IActionFilter,          // runs third & fourth
                                IResultFilter,          // runs fifth
                                // IEndpointFilter,        // runs only if action method runs ok
                                IExceptionFilter        // runs if exception is thrown
{
    public LoggingFilter(ILogger<LoggingFilter> logger) : base(logger) {}

    public void OnActionExecuting(ActionExecutingContext context)
    {
        LogBefore(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        LogAfter(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        LogBefore(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        LogAfter(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }

    public void OnException(ExceptionContext context)
    {
        LogBefore(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
        // context.ExceptionHandled = true; // prevent the exception from bubbling up to default handler (aync one runs first)
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        LogBefore(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        LogAfter(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        LogBefore(System.Reflection.MethodBase.GetCurrentMethod()!.Name, context);
    }
}
