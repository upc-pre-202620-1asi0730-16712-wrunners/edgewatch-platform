namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

/// <summary>Creates the standard templates of an organization the first time it opens Reporting.</summary>
public record SeedDefaultReportTemplatesCommand(int OrganizationId, int OwnerUserId);
