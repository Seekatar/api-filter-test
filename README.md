# Exploring Middleware and MVC Filters in ASP.NET

This is a sample app that demonstrates all the flavors of ASP.NET Middleware and Filters.

## MVC Filters

Each `stage` has an interface. For any given controller method, there may be multiple filters for each stage.

Order is like a stack. Global, Controller, Action is the stack and when the controller method is run the order is reversed.

Can add a filter:

1. Globally by registering when `Program.cs` call `AddControllers`.
2. Attaching a filter attribute to a controller class.
3. Attaching a filter attribute to a controller's method.
4. Overriding a `Controller`-derived classes `OnActionExecuting` or `OnActionExecuted` or `OnActionExecutionAsync` methods.

Each has sync and async versions.

| Filter               | In/Out | Purpose                                                      |
| -------------------- | ------ | ------------------------------------------------------------ |
| IAuthorizationFilter | In     | Short-circuit the pipeline if the request is not authorized. |
| IResourceFilter      | In     | Caching, can short-circuit the pipeline.                     |
| IActionFilter        | In     | Run after model binding, so can view and modify input.       |
| IActionFilter        | Out    | Run after the controller method. Can modify the output.      |
| IResultFilter        | Out    | Runs before and after "Result execution"                     |
| IExceptionFilter     | n/a    | Runs when an exception is thrown. Can modify the response.   |
| IOrderedFilter       | n/a    | Can override default ordering of filter processing           |

## Contexts

| Method                                     | Context Class                    | Context Properties                                                     |
| ------------------------------------------ | -------------------------------- | ---------------------------------------------------------------------- |
| IAuthorizationFilter.OnAuthorization       | AuthorizationFilterContextSealed | ActionDescriptor,Filters,HttpContext,ModelState,Result,RouteData       |
| ├─ IResourceFilter.OnResourceExecuting     | ResourceExecutingContextSealed   | + ValueProviderFactories                                               |
| ├── IActionFilter.OnActionExecuting        | ActionExecutingContextSealed     | + ActionArguments,Controller                                           |
| ├─── Controllers.WeatherForecastController | Controller method getting called |
| ├── IActionFilter.OnActionExecuted         | ActionExecutedContextSealed      | + Canceled,Controller,Exception,ExceptionDispatchInfo,ExceptionHandled |
| ├─ IResultFilter.OnResultExecuting         | ResultExecutingContextSealed     | + Cancel,Controller,                                                   |
| ├─ IResultFilter.OnResultExecuted          | ResultExecutedContextSealed      | + Canceled,Controller,Exception,ExceptionDispatchInfo,ExceptionHandled |
| └─ IResourceFilter.OnResourceExecuted      | ResourceExecutedContextSealed    | + Canceled,Exception,ExceptionDispatchInfo,ExceptionHandled            |

## Filter Attributes

All implement the [IOrderFilter](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.iorderedfilter?view=aspnetcore-7.0) for changing default ordering, and marker interface [IFilterMetadata](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.ifiltermetadata)

| Filter                                                                                                                             | Implements                                |
| ---------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| [ActionFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.actionfilterattribute)       | I(Async)ActionFilter,I(Async)ResultFilter |
| [ResultFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.resultfilterattribute)       | I(Async)ResultFilter                      |
| [ExceptionFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.exceptionfilterattribute) | I(Async)ExceptionFilter                   |
[FormatFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.formatfilterattribute)         | IFilterFactory|
[ServiceFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.servicefilterattribute)         | IFilterFactory|
[TypeFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.typefilterattribute)         | IFilterFactory|

## Links

- [MS Doc: Filters](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters)
- [MS Doc: Middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware)
