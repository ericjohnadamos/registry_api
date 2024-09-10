using System.Collections.Generic;

namespace RegistryApi.Web.Errors;

/// <summary>
///     Represents an error response from an API requests. Wraps a sequence of <see cref="ApiError" /> instances
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    ///     Gets or sets the sequence of <see cref="ApiError" /> instances
    /// </summary>
    public IEnumerable<ApiError> Errors { get; set; } = new List<ApiError>();
}