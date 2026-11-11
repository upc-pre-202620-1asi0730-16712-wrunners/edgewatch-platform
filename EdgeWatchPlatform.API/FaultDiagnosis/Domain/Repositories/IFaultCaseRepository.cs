using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;

public interface IFaultCaseRepository : IBaseRepository<FaultCase>
{
    Task<IEnumerable<FaultCase>> FindByOrganizationIdAsync(int organizationId, int? spraySessionId, FaultCaseStatus? status, CancellationToken cancellationToken = default);

    /// <summary>Latest case of the same session, parameter and band (for the cooldown).</summary>
    Task<FaultCase?> FindLatestBySpraySessionIdAndParameterAndBandAsync(int spraySessionId, string parameter, FaultBand band, CancellationToken cancellationToken = default);
}
