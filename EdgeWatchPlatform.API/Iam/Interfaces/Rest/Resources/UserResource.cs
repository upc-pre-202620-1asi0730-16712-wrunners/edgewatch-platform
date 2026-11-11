namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

public record UserResource(int Id, string FullName, string Email, int OrganizationId, string Status, IEnumerable<RoleIdResource> RoleIds);
