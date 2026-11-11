using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Commands;

/// <summary>Registers an organization together with its first user, who becomes ROLE_ORG_ADMIN.</summary>
public record SignUpCommand(string OrganizationName, Ruc Ruc, OrganizationType OrganizationType, string TimeZone,
    string FullName, EmailAddress Email, string Password);
