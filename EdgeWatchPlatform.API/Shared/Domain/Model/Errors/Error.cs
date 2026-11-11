namespace EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

/// <summary>Classification of a domain error, used to derive the HTTP status of a problem details response.</summary>
public enum EErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unexpected = 6
}

/// <summary>A domain error with a unique code, a default English message and a classification.</summary>
/// <param name="Code">Unique code, e.g. <c>Equipment.DuplicateHvofSystemCode</c>. Also the localization key of the detail.</param>
/// <param name="Message">Default message when no localized text exists.</param>
/// <param name="Type">Classification of the error.</param>
public record Error(string Code, string Message, EErrorType Type = EErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public static Error NotFound(string code, string message) => new(code, message, EErrorType.NotFound);
    public static Error Conflict(string code, string message) => new(code, message, EErrorType.Conflict);
    public static Error Validation(string code, string message) => new(code, message, EErrorType.Validation);
    public static Error Failure(string code, string message) => new(code, message, EErrorType.Failure);
    public static Error Unauthorized(string code, string message) => new(code, message, EErrorType.Unauthorized);
    public static Error Forbidden(string code, string message) => new(code, message, EErrorType.Forbidden);
    public static Error Unexpected(string code, string message) => new(code, message, EErrorType.Unexpected);
}
