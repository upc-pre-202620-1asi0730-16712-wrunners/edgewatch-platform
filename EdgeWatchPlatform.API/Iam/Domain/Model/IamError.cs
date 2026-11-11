namespace EdgeWatchPlatform.API.Iam.Domain.Model;

/// <summary>Errors of the IAM bounded context, mapped to HTTP status codes by <c>IamActionResultAssembler</c>.</summary>
public enum IamError
{
    None,
    UserNotFound,
    OrganizationNotFound,
    RoleNotFound,
    EmailAlreadyTaken,
    RucAlreadyTaken,
    InvalidCredentials,
    Unauthorized,
    Forbidden,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
