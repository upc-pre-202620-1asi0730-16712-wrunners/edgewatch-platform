using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user) =>
        new(user.Id, user.FullName, user.Email.Value, user.OrganizationId, user.Status.ToUpperSnakeCase(),
            user.Roles.Select(r => new RoleIdResource(r.RoleId)).ToList());
}
