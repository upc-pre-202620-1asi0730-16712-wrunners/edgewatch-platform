using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Reporting.Application.QueryServices;

public interface IGeneratedReportQueryService
{
    Task<IEnumerable<GeneratedReport>> Handle(GetAllGeneratedReportsByOrganizationIdQuery query, CancellationToken cancellationToken = default);
    Task<GeneratedReport?> Handle(GetGeneratedReportByIdQuery query, CancellationToken cancellationToken = default);
}
