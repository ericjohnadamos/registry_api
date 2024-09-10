using System;
using System.Collections.Generic;
using System.Net;
using RegistryApi.Core.Users.Exceptions;
using RegistryApi.SharedKernel.Exceptions;

namespace RegistryApi.Web.Errors;

/// <summary>
///     Class for creating <see cref="ApiError" /> from exceptions
/// </summary>
public class ApiErrorFactory
{
    public (IEnumerable<ApiError> ApiErrors, HttpStatusCode StatusCode) ParseException(Exception exception)
    {
        switch (exception)
        {
            case NotFoundException notFoundException:
                return ParseException(notFoundException);
            case UserExistsException userExistsException:
                return ParseException(userExistsException);
            default:
                return (new List<ApiError> { new ApiError("An unhandled exception has occurred. Please try your request again, or contact support.") },
                    HttpStatusCode.InternalServerError);
        }
    }

    private static (IEnumerable<ApiError> ApiErrors, HttpStatusCode StatusCode) ParseException(NotFoundException keyNotFoundException)
        => ParseExceptionMessage(keyNotFoundException, HttpStatusCode.NotFound);

    private static (IEnumerable<ApiError> ApiErrors, HttpStatusCode StatusCode) ParseException(UserExistsException userExistsException)
        => ParseExceptionMessage(userExistsException, HttpStatusCode.Conflict);

    private static (IEnumerable<ApiError> ApiErrors, HttpStatusCode StatusCode) ParseExceptionMessage(Exception exception, HttpStatusCode httpStatusCode)
        => (new List<ApiError>
        {
            new ApiError(exception.Message)
        }, httpStatusCode);
}