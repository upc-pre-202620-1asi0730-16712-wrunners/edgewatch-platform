using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Reporting.Application.QueryServices;

public interface IReportTemplateQueryService
{
    Task<IEnumerable<ReportTemplate>> Handle(GetAllReportTemplatesByOrganizationIdQuery query, CancellationToken cancellationToken = default);
    Task<ReportTemplate?> Handle(GetReportTemplateByIdQuery query, CancellationToken cancellationToken = default);
}
