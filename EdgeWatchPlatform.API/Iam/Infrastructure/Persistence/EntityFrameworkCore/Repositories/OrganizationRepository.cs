using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class OrganizationRepository(AppDbContext context) : BaseRepository<Organization>(context), IOrganizationRepository
{
    public async Task<bool> ExistsByRucAsync(Ruc ruc, CancellationToken cancellationToken = default) =>
        await Context.Set<Organization>().AnyAsync(o => o.Ruc == ruc, cancellationToken);
}
