using Microsoft.AspNetCore.Mvc.Filters;

internal class LoggingExceptionAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingExceptionAttribute(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    override public void OnException(ExceptionContext context)
    {
        LoggingFilterBase.LogBefore(_logger, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod()!.Name, context, this);
    }
}