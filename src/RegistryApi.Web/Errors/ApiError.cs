namespace RegistryApi.Web.Errors;

/// <summary>
///     Represents an error returned from an API request
/// </summary>
public class ApiError
{
    public ApiError()
    {
    }

    public ApiError(string message)
    {
        Message = message;
    }

    /// <summary>
    ///     Gets or sets the api error message
    /// </summary>
    public string Message { get; private set; } = string.Empty;
}