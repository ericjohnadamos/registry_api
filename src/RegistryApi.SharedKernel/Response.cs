namespace RegistryApi.SharedKernel;

using System;

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

public sealed class SuccessResponse<T>(T result) : Response<T>(result)
{
}

public sealed class FailureResponse<T>(Exception exception) : Response<T>(exception)
{
}