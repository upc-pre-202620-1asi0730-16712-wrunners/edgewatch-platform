using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Errors;

/// <summary>Domain errors of the IAM bounded context.</summary>
public static class IamErrors
{
    public static readonly Error UserNotFound = Error.NotFound("Iam.UserNotFound", "The user {0} was not found.");
    public static readonly Error OrganizationNotFound = Error.NotFound("Iam.OrganizationNotFound", "The organization {0} was not found.");
    public static readonly Error RoleNotFound = Error.NotFound("Iam.RoleNotFound", "The role {0} was not found.");
    public static readonly Error EmailAlreadyTaken = Error.Conflict("Iam.EmailAlreadyTaken", "The email {0} is already registered.");
    public static readonly Error RucAlreadyTaken = Error.Conflict("Iam.RucAlreadyTaken", "An organization with RUC {0} already exists.");
    public static readonly Error InvalidCredentials = Error.Unauthorized("Iam.InvalidCredentials", "Invalid email or password.");
    public static readonly Error Unauthorized = Error.Unauthorized("Iam.Unauthorized", "A valid bearer token is required.");
    public static readonly Error Forbidden = Error.Forbidden("Iam.Forbidden", "The current user is not allowed to perform this operation.");
    public static readonly Error DatabaseError = Error.Unexpected("Iam.DatabaseError", "A database error occurred during the IAM operation.");
    public static readonly Error InternalServerError = Error.Unexpected("Iam.InternalServerError", "An unexpected error occurred in IAM.");
}
