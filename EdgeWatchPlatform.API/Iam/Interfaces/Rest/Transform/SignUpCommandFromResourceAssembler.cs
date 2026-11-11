using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource) =>
        new(resource.OrganizationName, new Ruc(resource.Ruc), resource.OrganizationType.ToEnum<OrganizationType>(),
            resource.TimeZone ?? "America/Lima", resource.FullName, new EmailAddress(resource.Email), resource.Password);
}
