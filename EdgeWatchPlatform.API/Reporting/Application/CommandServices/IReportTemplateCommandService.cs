using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.Reporting.Application.CommandServices;

public interface IReportTemplateCommandService
{
    Task<Result<ReportTemplate>> Handle(CreateReportTemplateCommand command, CancellationToken cancellationToken = default);
    Task<Result<ReportTemplate>> Handle(UpdateReportTemplateCommand command, CancellationToken cancellationToken = default);
    Task<Result<ReportTemplate>> Handle(ChangeReportTemplateStatusCommand command, CancellationToken cancellationToken = default);
    Task Handle(SeedDefaultReportTemplatesCommand command, CancellationToken cancellationToken = default);
}
