using RegistryApi.Web.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RegistryApi.Web;

/// <summary>
///     Filter for processing unhandled exceptions that occur in the ASP.NET MVC pipeline
/// </summary>
/// <remarks>
///     Filter for processing unhandled exceptions that occur in the ASP.NET MVC pipeline
/// </remarks>
/// <param name="logger"> Logger instance for the <see cref="UnhandledApiExceptionFilter" />. </param>
/// <param name="apiErrorFactory"> Factory for converting exceptions into <see cref="ApiError" />. </param>
public class UnhandledApiExceptionFilter(ILogger<UnhandledApiExceptionFilter> logger, ApiErrorFactory apiErrorFactory)
        : IExceptionFilter
{

    /// <summary>
    ///     Occurs when the exception is thrown
    /// </summary>
    /// <param name="context"> Information about the current unhandled exception </param>
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, message: context.Exception.Message);
        var (ApiErrors, StatusCode) = apiErrorFactory.ParseException(context.Exception);

        var jsonResult = new JsonResult(new ApiErrorResponse { Errors = ApiErrors })
        {
            StatusCode = (int) StatusCode
        };
        context.Result = jsonResult;
    }
}