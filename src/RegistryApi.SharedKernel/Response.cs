using System;

namespace RegistryApi.SharedKernel;

public abstract class Response<T>
{
    private Response(T result, Exception exception)
    {
        Result = result;
        Exception = exception;
    }

    private protected Response(T result) : this(result, null)
    {
    }

    private protected Response(Exception exception) : this(default, exception)
    {
    }

    public T Result { get; }
    public Exception Exception { get; }
    public bool WasSuccessful => Exception is null;

    public static Response<T> Success(T result) => new SuccessResponse<T>(result);
    public static Response<T> Failure(Exception ex) => new FailureResponse<T>(ex);
}

public sealed class SuccessResponse<T> : Response<T>
{
    public SuccessResponse(T result)
        : base(result)
    {
    }
}

public sealed class FailureResponse<T> : Response<T>
{
    public FailureResponse(Exception exception)
        : base(exception)
    {
    }
}