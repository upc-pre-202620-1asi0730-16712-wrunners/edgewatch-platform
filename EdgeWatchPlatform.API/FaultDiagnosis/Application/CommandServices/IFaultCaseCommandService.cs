using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;

public interface IFaultCaseCommandService
{
    /// <summary>Opens a case when a rule matches and the cooldown elapsed; null otherwise. Does not commit: it runs inside the readings unit of work.</summary>
    Task<FaultCase?> Handle(OpenFaultCaseCommand command, CancellationToken cancellationToken = default);

    Task<Result<FaultCase>> Handle(AcknowledgeFaultCaseCommand command, CancellationToken cancellationToken = default);
    Task<Result<FaultCase>> Handle(ResolveFaultCaseCommand command, CancellationToken cancellationToken = default);
}
