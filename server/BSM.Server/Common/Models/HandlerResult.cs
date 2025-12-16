namespace BSM.Server.Common.Models;

public class HandlerResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public IDictionary<string, string[]>? ValidationErrors { get; }

    protected HandlerResult(bool isSuccess, string? error, IDictionary<string, string[]>? validationErrors)
    {
        IsSuccess = isSuccess;
        Error = error;
        ValidationErrors = validationErrors;
    }

    public static HandlerResult Success() => new(true, null, null);

    public static HandlerResult Failure(string error) => new(false, error, null);
    
    public static HandlerResult ValidationFailure(IDictionary<string, string[]> validationErrors) 
        => new(false, "One or more validation errors occurred.", validationErrors);
}

public class HandlerResult<T> : HandlerResult
{
    public T? Data { get; }

    private HandlerResult(bool isSuccess, T? data, string? error, IDictionary<string, string[]>? validationErrors) 
        : base(isSuccess, error, validationErrors)
    {
        Data = data;
    }

    public static HandlerResult<T> Success(T data) => new(true, data, null, null);

    public new static HandlerResult<T> Failure(string error) => new(false, default, error, null);

    public new static HandlerResult<T> ValidationFailure(IDictionary<string, string[]> validationErrors) 
        => new(false, default, "One or more validation errors occurred.", validationErrors);
}
