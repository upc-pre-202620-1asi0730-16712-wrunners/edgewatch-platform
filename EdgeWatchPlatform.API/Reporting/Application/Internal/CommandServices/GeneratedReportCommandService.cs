using EdgeWatchPlatform.API.Reporting.Application.CommandServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Errors;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.CommandServices;

public class GeneratedReportCommandService(IGeneratedReportRepository reportRepository, IReportTemplateRepository templateRepository, IUnitOfWork unitOfWork,
    ILogger<GeneratedReportCommandService> logger) : IGeneratedReportCommandService
{
    public async Task<Result<GeneratedReport>> Handle(RegisterGeneratedReportCommand command, CancellationToken cancellationToken = default)
    {
        var template = await templateRepository.FindByIdAsync(command.ReportTemplateId, cancellationToken);
        if (template is null || template.OrganizationId != command.OrganizationId)
            return Result<GeneratedReport>.Failure(ReportingErrors.ReportTemplateNotFound, ReportingError.ReportTemplateNotFound);
        if (!template.IsActive) return Result<GeneratedReport>.Failure(ReportingErrors.ReportTemplateArchived, ReportingError.ReportTemplateArchived);

        try
        {
            var report = new GeneratedReport(command);
            await reportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<GeneratedReport>.Success(report);
        }
        catch (ArgumentException ex)
        {
            return Result<GeneratedReport>.Failure(ReportingErrors.InvalidTemplateLayout(ex.Message), ReportingError.InvalidTemplateLayout);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error registering a generated report for template {ReportTemplateId}", command.ReportTemplateId);
            return Result<GeneratedReport>.Failure(ReportingErrors.InternalServerError, ReportingError.InternalServerError);
        }
    }
}
