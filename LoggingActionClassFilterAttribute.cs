using Microsoft.AspNetCore.Mvc.Filters;

internal class LoggingActionClassFilterAttribute : ActionFilterAttribute
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingActionClassFilterAttribute(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }
    override public void OnActionExecuting(ActionExecutingContext context)
    {
        LoggingFilterBase.LogBefore(_logger, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
    }
    override public void OnActionExecuted(ActionExecutedContext context)
    {
        LoggingFilterBase.LogAfter(_logger, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
    }
    override public void OnResultExecuting(ResultExecutingContext context)
    {
        LoggingFilterBase.LogBefore(_logger, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
    }
    override public void OnResultExecuted(ResultExecutedContext context)
    {
        LoggingFilterBase.LogAfter(_logger, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
    }
}