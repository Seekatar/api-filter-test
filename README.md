# Exploring Middleware and MVC Filters in ASP.NET

This is a sample app that demonstrates all the flavors of ASP.NET Middleware and Filters. These filters just do some logging for demonstration purposes.

## MVC Filters

Each `stage` has an interface. For any given controller method, there may be multiple filters for each stage.

Order is like a stack. Global, Controller, Action is the stack and when the controller method is run the order is reversed.

MVC code groups by 'type', and then ordered within each group. Authorization always runs first.(Default `Order` = -1)

- ActionFilter
- AuthorizationFilter - called first to short-circuit the pipeline of not authorized.
- ExceptionFilter (MS says to prefer middleware for this)
- ResultFilter - called when an ActionResult is returned.

In before filter can short-circuit the pipeline by setting `Result` to a non-null value. In an after filter can modify the output by modifying `Result`.

Can add a filter:

1. Globally by registering when `Program.cs` call `AddControllers`.
2. Attaching a filter attribute to a controller class.
3. Attaching a filter attribute to a controller's method.
4. Overriding a `Controller`-derived classes `OnActionExecuting` or `OnActionExecuted` or `OnActionExecutionAsync` methods.

Each has sync and async versions.

| Interface                   | Run    | Purpose                                                                          |
| --------------------------- | ------ | -------------------------------------------------------------------------------- |
| I(Async)AuthorizationFilter | Before | Short-circuit the pipeline if the request is not authorized. By setting `Result` |
| I(Async)ResourceFilter      | Before | Caching, can short-circuit the pipeline.                                         |
| I(Async)ActionFilter        | Before | Run after model binding, so can view and modify input.                           |
| I(Async)ActionFilter        | After  | Run after the controller method. Can modify the output.                          |
| I(Async)ResultFilter        | After  | Runs before and after "Result execution"                                         |
| I(Async)ExceptionFilter     | n/a    | Runs when an exception is thrown. Can modify the response.                       |
| IOrderedFilter              | n/a    | Can override default ordering of filter processing                               |

Not covered by this sample:

| Interface                     | Run | Purpose                                                                       |
| ----------------------------- | --- | ----------------------------------------------------------------------------- |
| I(Async)AlwaysRunResultFilter | n/a | Always runs after all other filters, even if the pipeline is short-circuited. |
| IFilterFactory                | n/a | Can create a filter instance with DI for more dynamic filtering.              |

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
| [FormatFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.formatfilterattribute)       | IFilterFactory                            |
| [ServiceFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.servicefilterattribute)             | IFilterFactory                            |
| [TypeFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.typefilterattribute)                   | IFilterFactory                            |

## Using DI in Filters

Global filters that implement the interfaces and added via AddControllers can inject dependencies via constructor injection. Attribute filters that do not need DI, can be used like normal attributes, but if you need DI, then you can use the `ServiceFilterAttribute` or `TypeFilterAttribute` to inject dependencies. This sample uses both.

`TypeFilterAttribute` allows passing in non-service arguments in the attribute.

## Links

- [MS Doc: Filters](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters)
- [MS Doc: Middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware)
- [MS Sample code for filters](https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/mvc/controllers/filters/samples/6.x/FiltersSample)
- [Custom Ordering of Action Filters in ASP.NET MVC
](https://gregshackles.com/custom-ordering-of-action-filters-in-asp-net-mvc/) Greg Shackles builds a framework for custom ordering of filters.
