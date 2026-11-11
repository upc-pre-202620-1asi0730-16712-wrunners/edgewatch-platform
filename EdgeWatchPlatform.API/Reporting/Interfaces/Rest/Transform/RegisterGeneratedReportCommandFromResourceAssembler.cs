using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class RegisterGeneratedReportCommandFromResourceAssembler
{
    public static RegisterGeneratedReportCommand ToCommandFromResource(int organizationId, int userId, RegisterGeneratedReportResource resource) =>
        new(resource.ReportTemplateId, organizationId, userId, resource.SubjectType.ToEnum<ReportSubjectType>(), resource.SubjectId, resource.FileName,
            resource.PeriodFrom, resource.PeriodTo);
}
