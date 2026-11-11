namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

public record UpdateFaultRuleCommand(int FaultRuleId, string ProbableCause, string RecommendedAction, int CooldownSeconds);
