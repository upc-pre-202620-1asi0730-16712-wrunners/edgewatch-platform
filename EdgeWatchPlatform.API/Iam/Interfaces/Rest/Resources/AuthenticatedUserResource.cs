namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

/// <summary>Answer of a sign-in: the user, its organization type and the bearer token.</summary>
public record AuthenticatedUserResource(int Id, string Email, string FullName, int OrganizationId, string OrganizationType,
    IEnumerable<RoleIdResource> RoleIds, string Token);
