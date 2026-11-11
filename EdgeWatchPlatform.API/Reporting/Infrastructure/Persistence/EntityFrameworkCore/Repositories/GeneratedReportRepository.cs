using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GeneratedReportRepository(AppDbContext context) : BaseRepository<GeneratedReport>(context), IGeneratedReportRepository
{
    public async Task<IEnumerable<GeneratedReport>> FindByOrganizationIdAsync(int organizationId, ReportSubjectType? subjectType, int? subjectId, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<GeneratedReport>().Where(r => r.OrganizationId == organizationId);
        if (subjectType is { } type) query = query.Where(r => r.SubjectType == type);
        if (subjectId is { } id) query = query.Where(r => r.SubjectId == id);
        return await query.OrderByDescending(r => r.GeneratedAt).ToListAsync(cancellationToken);
    }
}
