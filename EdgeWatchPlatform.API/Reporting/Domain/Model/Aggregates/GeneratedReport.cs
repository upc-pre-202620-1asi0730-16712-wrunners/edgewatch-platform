using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Events;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;

/// <summary>The record of a report rendered from a template: who generated it, about what and when.</summary>
public partial class GeneratedReport : AggregateRoot
{
    protected GeneratedReport()
    {
        FileName = string.Empty;
        Format = "PDF";
    }

    public GeneratedReport(RegisterGeneratedReportCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.FileName);
        if (command.PeriodFrom is { } from && command.PeriodTo is { } to && to < from)
            throw new ArgumentException("The period end cannot precede its start.", nameof(command));
        ReportTemplateId = command.ReportTemplateId;
        OrganizationId = command.OrganizationId;
        GeneratedByUserId = command.GeneratedByUserId;
        SubjectType = command.SubjectType;
        SubjectId = command.SubjectId;
        FileName = command.FileName.Trim();
        PeriodFrom = command.PeriodFrom;
        PeriodTo = command.PeriodTo;
        GeneratedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new ReportGeneratedEvent(Id, ReportTemplateId, OrganizationId, SubjectType.ToUpperSnakeCase(), SubjectId));
    }

    public int Id { get; private set; }
    public int ReportTemplateId { get; private set; }
    public int OrganizationId { get; private set; }
    public int GeneratedByUserId { get; private set; }
    public ReportSubjectType SubjectType { get; private set; }
    public int SubjectId { get; private set; }
    public string Format { get; private set; }
    public string FileName { get; private set; }
    public DateOnly? PeriodFrom { get; private set; }
    public DateOnly? PeriodTo { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
}
