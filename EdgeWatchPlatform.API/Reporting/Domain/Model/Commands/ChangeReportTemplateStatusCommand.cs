using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

public record ChangeReportTemplateStatusCommand(int ReportTemplateId, TemplateStatus Status);
