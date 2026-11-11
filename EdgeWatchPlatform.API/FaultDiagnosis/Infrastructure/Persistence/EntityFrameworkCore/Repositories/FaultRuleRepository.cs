using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class FaultRuleRepository(AppDbContext context) : BaseRepository<FaultRule>(context), IFaultRuleRepository
{
    public async Task<IEnumerable<FaultRule>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<FaultRule>().Where(r => r.OrganizationId == organizationId).OrderBy(r => r.Parameter).ThenBy(r => r.Band).ToListAsync(cancellationToken);

    public async Task<FaultRule?> FindByOrganizationIdAndParameterAndBandAsync(int organizationId, string parameter, FaultBand band, CancellationToken cancellationToken = default) =>
        await Context.Set<FaultRule>().FirstOrDefaultAsync(r => r.OrganizationId == organizationId && r.Parameter == parameter && r.Band == band, cancellationToken);

    public async Task<bool> ExistsByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<FaultRule>().AnyAsync(r => r.OrganizationId == organizationId, cancellationToken);
}
