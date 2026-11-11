using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, Organization organization, string token) =>
        new(user.Id, user.Email.Value, user.FullName, user.OrganizationId, organization.OrganizationType.ToUpperSnakeCase(),
            user.Roles.Select(r => new RoleIdResource(r.RoleId)).ToList(), token);
}
