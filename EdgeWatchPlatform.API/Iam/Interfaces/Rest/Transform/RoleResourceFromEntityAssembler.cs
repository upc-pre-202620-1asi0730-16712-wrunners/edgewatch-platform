using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class RoleResourceFromEntityAssembler
{
    public static RoleResource ToResourceFromEntity(Role role) => new(role.Id, role.StringName);
}
