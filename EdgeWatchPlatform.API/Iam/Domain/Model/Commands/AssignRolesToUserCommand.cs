namespace EdgeWatchPlatform.API.Iam.Domain.Model.Commands;

/// <summary>Replaces the roles of a user with the given role ids.</summary>
public record AssignRolesToUserCommand(int UserId, IReadOnlyCollection<int> RoleIds);
