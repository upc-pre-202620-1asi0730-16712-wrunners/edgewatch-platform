using EdgeWatchPlatform.API.Iam.Application.CommandServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.CommandServices;

public class RoleCommandService(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRoleCommandService
{
    /// <summary>Seeds every role of <see cref="Roles"/> that is missing, in declaration order so ids match the web application.</summary>
    public async Task Handle(SeedRolesCommand command, CancellationToken cancellationToken = default)
    {
        foreach (var name in Enum.GetValues<Roles>())
        {
            if (await roleRepository.ExistsByNameAsync(name, cancellationToken)) continue;
            await roleRepository.AddAsync(new Role(name), cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
