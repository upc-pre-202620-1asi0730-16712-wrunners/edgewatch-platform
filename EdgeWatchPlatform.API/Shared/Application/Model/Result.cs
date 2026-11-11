using DomainError = EdgeWatchPlatform.API.Shared.Domain.Model.Errors.Error;

namespace EdgeWatchPlatform.API.Shared.Application.Model;

/// <summary>
///     Outcome of a command handled in the application layer: a value on success, or a domain error
///     plus the bounded context error enum on failure.
/// </summary>
/// <typeparam name="T">Type of the value on success.</typeparam>
public class Result<T>
{
    protected Result(bool isSuccess, T? value, string message, Enum? errorEnum, DomainError? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
        Error = errorEnum;
        DomainError = error ?? (isSuccess ? DomainError.None : DomainError.Failure(errorEnum?.ToString() ?? "Error", message));
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Message { get; }
    public Enum? Error { get; }
    public DomainError DomainError { get; }

    public static Result<T> Success(T value) => new(true, value, string.Empty, null, DomainError.None);

    public static Result<T> Failure(Enum error, string message) =>
        new(false, default, message, error, DomainError.Failure(error.ToString(), message));

    public static Result<T> Failure(DomainError error) => new(false, default, error.Message, null, error);

    public static Result<T> Failure(DomainError error, Enum errorEnum) => new(false, default, error.Message, errorEnum, error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<DomainError, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(DomainError);
}

/// <summary>Non-generic result for commands without a return value.</summary>
public class Result : Result<object>
{
    private Result(bool isSuccess, string message, Enum? errorEnum, DomainError? error)
        : base(isSuccess, null, message, errorEnum, error)
    {
    }

    public static Result Success() => new(true, string.Empty, null, DomainError.None);

    public new static Result Failure(Enum error, string message) =>
        new(false, message, error, DomainError.Failure(error.ToString(), message));

    public new static Result Failure(DomainError error) => new(false, error.Message, null, error);

    public new static Result Failure(DomainError error, Enum errorEnum) => new(false, error.Message, errorEnum, error);
}
