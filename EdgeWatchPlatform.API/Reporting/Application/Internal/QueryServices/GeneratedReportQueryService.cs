using EdgeWatchPlatform.API.Reporting.Application.QueryServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.QueryServices;

public class GeneratedReportQueryService(IGeneratedReportRepository reportRepository) : IGeneratedReportQueryService
{
    public async Task<IEnumerable<GeneratedReport>> Handle(GetAllGeneratedReportsByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await reportRepository.FindByOrganizationIdAsync(query.OrganizationId, query.SubjectType, query.SubjectId, cancellationToken);

    public async Task<GeneratedReport?> Handle(GetGeneratedReportByIdQuery query, CancellationToken cancellationToken = default) =>
        await reportRepository.FindByIdAsync(query.GeneratedReportId, cancellationToken);
}
