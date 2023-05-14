using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

internal class LoggingFilterBase
{
    private readonly ILogger _logger;
    List<IDisposable> _scopes = new();
    protected LoggingFilterBase(ILogger logger)
    {
        _logger = logger;
    }

    const string AttrDepth = "LoggingFilterBase.Depth";
    static string Indent(int depth) => new string(' ', depth * 2);

    private static void LogType<T>(ILogger logger, T context, IOrderedFilter? orderFilter, int depth)
    {
        if (context == null)
            return;
        var type = context.GetType();
        // get all the property names of the type
        var propertyNames = type.GetProperties().Select(p => p.Name);
        logger.LogInformation($"{Indent(depth)}  {orderFilter?.Order ?? -99} {type.Name}: {string.Join(",", propertyNames.OrderBy(p => p))}");
    }

    protected void LogBefore<T>(string name, T context, IOrderedFilter? orderFilter = null) where T : ActionContext
    {
        _scopes.Add(_logger.BeginScope(new Dictionary<string, string> { { "method", name } }));
        LogBefore(_logger, GetType().Name, name, context, orderFilter);
    }

    protected void LogAfter<T>(string name, T context, IOrderedFilter? orderFilter = null) where T : ActionContext
    {
        LogAfter(_logger, GetType().Name, name, context, orderFilter);
    }

    internal static void LogAfter<T>(ILogger logger, string typeName, string name, T context, IOrderedFilter? orderFilter = null) where T : ActionContext
    {
        var depth = 0;
        if (!context.HttpContext.Items.TryGetValue(AttrDepth, out var objDepth ) || objDepth == null)
        {
            depth = 1;
            context.HttpContext.Items[AttrDepth] = 1;
        }
        else
        {
            depth = (int)objDepth;
            context.HttpContext.Items[AttrDepth] = depth - 1;
        }
        logger.LogInformation($"{Indent(depth)}{typeName}.{name}");
        LogType(logger, context, orderFilter, depth);
    }

    internal static void LogBefore<T>(ILogger logger, string typeName, string name, T context, IOrderedFilter? orderFilter = null) where T : ActionContext
    {
        var depth = 0;
        if (!context.HttpContext.Items.TryGetValue(AttrDepth, out var objDepth ) || objDepth == null)
        {
            depth = 1;
            context.HttpContext.Items[AttrDepth] = 1;
        }
        else
        {
            depth = (int)objDepth + 1;
            context.HttpContext.Items[AttrDepth] = depth;
        }

        logger.LogInformation($"{Indent(depth)}{typeName}.{name}");
        LogType(logger, context, orderFilter, depth);
    }
}
