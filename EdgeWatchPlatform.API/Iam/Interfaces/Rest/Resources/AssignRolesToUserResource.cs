using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

public record AssignRolesToUserResource([Required] [MinLength(1, ErrorMessage = "RequiredRoleIds")] IReadOnlyCollection<RoleIdResource> RoleIds);
