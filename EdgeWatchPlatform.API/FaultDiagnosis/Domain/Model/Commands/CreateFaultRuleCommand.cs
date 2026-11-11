using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

public record CreateFaultRuleCommand(int OrganizationId, string Parameter, FaultBand Band, string ProbableCause, string RecommendedAction, int CooldownSeconds);
