using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class GeneratedReportResourceFromEntityAssembler
{
    public static GeneratedReportResource ToResourceFromEntity(GeneratedReport report) =>
        new(report.Id, report.ReportTemplateId, report.OrganizationId, report.GeneratedByUserId, report.SubjectType.ToUpperSnakeCase(), report.SubjectId,
            report.Format, report.FileName, report.PeriodFrom, report.PeriodTo, report.GeneratedAt);
}
