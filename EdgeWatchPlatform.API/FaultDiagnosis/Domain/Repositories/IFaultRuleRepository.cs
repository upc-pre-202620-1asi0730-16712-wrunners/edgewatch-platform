using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;

public interface IFaultRuleRepository : IBaseRepository<FaultRule>
{
    Task<IEnumerable<FaultRule>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<FaultRule?> FindByOrganizationIdAndParameterAndBandAsync(int organizationId, string parameter, FaultBand band, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
}
