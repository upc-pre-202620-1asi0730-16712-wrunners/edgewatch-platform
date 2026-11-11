namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

/// <summary>Seeds the FESA default rules for an organization that has none yet.</summary>
public record SeedDefaultFaultRulesCommand(int OrganizationId);
