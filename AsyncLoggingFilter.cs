using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

internal class AsyncLoggingFilter :  LoggingFilterBase,
                                     IAsyncAuthorizationFilter,   // runs first
                                     IAsyncResourceFilter,        // runs second & sixth
                                     IAsyncActionFilter,          // runs third & fourth
                                     IAsyncResultFilter,          // runs fifth
                                     // IEndpointFilter,        // runs only if action method runs ok
                                     IAsyncExceptionFilter        // runs if exception is thrown

{
    public AsyncLoggingFilter(ILogger<LoggingFilter> logger) : base(logger)
    {
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var name = nameof(OnAuthorizationAsync);
        LogBefore(name, context);
        // short circuit it context.Result = new UnauthorizedResult();
        return Task.CompletedTask;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var name = nameof(OnActionExecutionAsync);
        LogBefore(name, context);
        var executedContext = await next();
        if (executedContext.Canceled) {} //  another filter short-circuited the pipeline
        if (executedContext.Exception is not null) {} //  something threw an exception
        LogAfter(name, context);
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var name = nameof(OnResultExecutionAsync);
        LogBefore(name, context);
        var executedContext = await next();
        LogAfter(name, context);
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var name = nameof(OnExceptionAsync);
        LogBefore(name, context);
        // context.ExceptionHandled = true; // this would prevent LoggingFilter.OnException from running
        return Task.CompletedTask;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var name = nameof(OnResourceExecutionAsync);
        LogBefore(name, context);
        var executedContext = await next();
        LogAfter(name, context);
    }
}
