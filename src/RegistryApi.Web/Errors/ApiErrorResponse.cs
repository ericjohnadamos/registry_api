namespace RegistryApi.Web.Errors;

using System.Collections.Generic;

/// <summary>
///     Represents an error response from an API requests. Wraps a sequence of <see cref="ApiError" /> instances
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    ///     Gets or sets the sequence of <see cref="ApiError" /> instances
    /// </summary>
    public IEnumerable<ApiError> Errors { get; set; } = [];
}