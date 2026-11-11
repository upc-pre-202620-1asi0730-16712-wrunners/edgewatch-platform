using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Reporting.Domain.Repositories;

public interface IReportTemplateRepository : IBaseRepository<ReportTemplate>
{
    Task<ReportTemplate?> FindByIdWithSectionsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> FindByOrganizationIdAsync(int organizationId, ReportKind? reportKind, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> FindDefaultsByOrganizationIdAndKindAsync(int organizationId, ReportKind reportKind, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrganizationIdAndNameAsync(int organizationId, string name, int? excludingId, CancellationToken cancellationToken = default);
}
