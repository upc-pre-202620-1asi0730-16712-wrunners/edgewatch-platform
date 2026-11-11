using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;

public interface IFaultRuleCommandService
{
    Task<Result<FaultRule>> Handle(CreateFaultRuleCommand command, CancellationToken cancellationToken = default);
    Task<Result<FaultRule>> Handle(UpdateFaultRuleCommand command, CancellationToken cancellationToken = default);
    Task Handle(SeedDefaultFaultRulesCommand command, CancellationToken cancellationToken = default);
}
