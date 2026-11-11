using EdgeWatchPlatform.API.Reporting.Application.QueryServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.QueryServices;

public class ReportTemplateQueryService(IReportTemplateRepository templateRepository) : IReportTemplateQueryService
{
    public async Task<IEnumerable<ReportTemplate>> Handle(GetAllReportTemplatesByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await templateRepository.FindByOrganizationIdAsync(query.OrganizationId, query.ReportKind, cancellationToken);

    public async Task<ReportTemplate?> Handle(GetReportTemplateByIdQuery query, CancellationToken cancellationToken = default) =>
        await templateRepository.FindByIdWithSectionsAsync(query.ReportTemplateId, cancellationToken);
}
