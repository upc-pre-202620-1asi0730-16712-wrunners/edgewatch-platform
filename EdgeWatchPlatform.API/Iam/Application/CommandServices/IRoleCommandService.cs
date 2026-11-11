using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;

namespace EdgeWatchPlatform.API.Iam.Application.CommandServices;

public interface IRoleCommandService
{
    Task Handle(SeedRolesCommand command, CancellationToken cancellationToken = default);
}
