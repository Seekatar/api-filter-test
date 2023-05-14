using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api_filter_test.Controllers;

[ApiController]
[Route("[controller]")]
[TypeFilter(typeof(LoggingActionClassFilterAttribute))] // since I need to inject a logger, I can't use [LoggingActionClassFilterAttribute] attribute
public class WeatherForecastController : Controller
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // Controller override
    // sync methods, too OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted, OnException
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        LoggingFilterBase.LogBefore(_logger, GetType().Name, nameof(OnActionExecutionAsync), context);
        await next();
        LoggingFilterBase.LogAfter(_logger, GetType().Name, nameof(OnActionExecutionAsync), context);
    }

    [HttpGet]
    [Route("/weather")]
    // Use TypeFilter since since I need to inject a logger
    [TypeFilter(typeof(LoggingActionFilterAttribute), Arguments = new object [] {"extraOverride"})]
    [TypeFilter(typeof(LoggingResultFilterAttribute), IsReusable=false)]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation(GetType().Name+"."+System.Reflection.MethodBase.GetCurrentMethod()!.Name);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("/error")]
    [TypeFilter(typeof(LoggingActionFilterAttribute))]
    [ServiceFilter(typeof(LoggingExceptionAttribute), IsReusable=true)] // since I need to inject a logger, I can't use [LoggingException] attribute
    public IEnumerable<WeatherForecast> Error()
    {
        _logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod()!.Name);

        throw new NotImplementedException("ow!");
    }
}
