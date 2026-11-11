using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Reporting.Domain.Repositories;

public interface IGeneratedReportRepository : IBaseRepository<GeneratedReport>
{
    Task<IEnumerable<GeneratedReport>> FindByOrganizationIdAsync(int organizationId, ReportSubjectType? subjectType, int? subjectId, CancellationToken cancellationToken = default);
}
