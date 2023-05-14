using Microsoft.AspNetCore.Mvc.Filters;

internal class LoggingActionFilterAttribute : ActionFilterAttribute
{
    private readonly ILogger<LoggingFilter> _logger;
    private readonly string _extra;

    public LoggingActionFilterAttribute(ILogger<LoggingFilter> logger, string extra = "extra")
    {
        _logger = logger;
        _extra = " "+extra;
    }
    override public void OnActionExecuting(ActionExecutingContext context)
    {
        LoggingFilterBase.LogBefore(_logger, GetType().Name+_extra, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
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