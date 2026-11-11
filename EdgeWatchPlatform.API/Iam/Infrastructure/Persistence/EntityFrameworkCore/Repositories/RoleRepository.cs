using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoleRepository(AppDbContext context) : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<Role?> FindByNameAsync(Roles name, CancellationToken cancellationToken = default) =>
        await Context.Set<Role>().FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    public async Task<bool> ExistsByNameAsync(Roles name, CancellationToken cancellationToken = default) =>
        await Context.Set<Role>().AnyAsync(r => r.Name == name, cancellationToken);

    public async Task<IEnumerable<Role>> FindByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var idList = ids.Distinct().ToList();
        return await Context.Set<Role>().Where(r => idList.Contains(r.Id)).ToListAsync(cancellationToken);
    }
}
