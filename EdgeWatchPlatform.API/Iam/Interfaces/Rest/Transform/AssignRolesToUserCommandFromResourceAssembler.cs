using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class AssignRolesToUserCommandFromResourceAssembler
{
    public static AssignRolesToUserCommand ToCommandFromResource(int userId, AssignRolesToUserResource resource) =>
        new(userId, resource.RoleIds.Select(r => r.RoleId).ToList());
}
