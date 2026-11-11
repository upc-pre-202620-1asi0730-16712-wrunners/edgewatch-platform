namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;

public record FaultRuleResource(int Id, int OrganizationId, string Parameter, string Band, string ProbableCause, string RecommendedAction, int CooldownSeconds);
