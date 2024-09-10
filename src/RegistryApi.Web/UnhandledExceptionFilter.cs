using RegistryApi.Web.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RegistryApi.Web;

/// <summary>
///     Filter for processing unhandled exceptions that occur in the ASP.NET MVC pipeline
/// </summary>
public class UnhandledApiExceptionFilter
    : IExceptionFilter
{
    private readonly ApiErrorFactory _apiErrorFactory;
    private readonly ILogger<UnhandledApiExceptionFilter> _logger;

    /// <summary>
    ///     Filter for processing unhandled exceptions that occur in the ASP.NET MVC pipeline
    /// </summary>
    /// <param name="logger"> Logger instance for the <see cref="UnhandledApiExceptionFilter" />. </param>
    /// <param name="apiErrorFactory"> Factory for converting exceptions into <see cref="ApiError" />. </param>
    public UnhandledApiExceptionFilter(ILogger<UnhandledApiExceptionFilter> logger, ApiErrorFactory apiErrorFactory)
    {
        _logger = logger;
        _apiErrorFactory = apiErrorFactory;
    }

    /// <summary>
    ///     Occurs when the exception is thrown
    /// </summary>
    /// <param name="context"> Information about the current unhandled exception </param>
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);
        var errorResults = _apiErrorFactory.ParseException(context.Exception);

        var jsonResult = new JsonResult(new ApiErrorResponse { Errors = errorResults.ApiErrors })
        {
            StatusCode = (int) errorResults.StatusCode
        };
        context.Result = jsonResult;
    }
}