using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReportTemplateRepository(AppDbContext context) : BaseRepository<ReportTemplate>(context), IReportTemplateRepository
{
    public async Task<ReportTemplate?> FindByIdWithSectionsAsync(int id, CancellationToken cancellationToken = default) =>
        await Context.Set<ReportTemplate>().Include(t => t.Sections).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IEnumerable<ReportTemplate>> FindByOrganizationIdAsync(int organizationId, ReportKind? reportKind, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<ReportTemplate>().Include(t => t.Sections).Where(t => t.OrganizationId == organizationId);
        if (reportKind is { } kind) query = query.Where(t => t.ReportKind == kind);
        return await query.OrderBy(t => t.ReportKind).ThenByDescending(t => t.DefaultForKind).ThenBy(t => t.Name).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReportTemplate>> FindDefaultsByOrganizationIdAndKindAsync(int organizationId, ReportKind reportKind, CancellationToken cancellationToken = default) =>
        await Context.Set<ReportTemplate>().Where(t => t.OrganizationId == organizationId && t.ReportKind == reportKind && t.DefaultForKind).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<ReportTemplate>().AnyAsync(t => t.OrganizationId == organizationId, cancellationToken);

    public async Task<bool> ExistsByOrganizationIdAndNameAsync(int organizationId, string name, int? excludingId, CancellationToken cancellationToken = default)
    {
        var trimmed = name.Trim();
        return await Context.Set<ReportTemplate>().AnyAsync(t => t.OrganizationId == organizationId && t.Name == trimmed && (excludingId == null || t.Id != excludingId), cancellationToken);
    }
}
