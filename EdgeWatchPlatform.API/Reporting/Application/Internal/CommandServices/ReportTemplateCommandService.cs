using EdgeWatchPlatform.API.Reporting.Application.CommandServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Errors;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.CommandServices;

public class ReportTemplateCommandService(IReportTemplateRepository templateRepository, IUnitOfWork unitOfWork, ILogger<ReportTemplateCommandService> logger)
    : IReportTemplateCommandService
{
    private const string DefaultPrimaryColor = "#0E1A2B";
    private const string DefaultSecondaryColor = "#D4781C";

    public async Task<Result<ReportTemplate>> Handle(CreateReportTemplateCommand command, CancellationToken cancellationToken = default)
    {
        if (await templateRepository.ExistsByOrganizationIdAndNameAsync(command.OrganizationId, command.Name, null, cancellationToken))
            return Result<ReportTemplate>.Failure(ReportingErrors.DuplicateReportTemplate, ReportingError.DuplicateReportTemplate);

        try
        {
            var template = new ReportTemplate(command);
            if (template.DefaultForKind) await ClearDefaults(command.OrganizationId, command.ReportKind, null, cancellationToken);
            await templateRepository.AddAsync(template, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ReportTemplate>.Success(template);
        }
        catch (ArgumentException ex)
        {
            return Result<ReportTemplate>.Failure(ReportingErrors.InvalidTemplateLayout(ex.Message), ReportingError.InvalidTemplateLayout);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error creating report template {Name}", command.Name);
            return Result<ReportTemplate>.Failure(ReportingErrors.DatabaseError, ReportingError.DatabaseError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating report template {Name}", command.Name);
            return Result<ReportTemplate>.Failure(ReportingErrors.InternalServerError, ReportingError.InternalServerError);
        }
    }

    public async Task<Result<ReportTemplate>> Handle(UpdateReportTemplateCommand command, CancellationToken cancellationToken = default)
    {
        var template = await templateRepository.FindByIdWithSectionsAsync(command.ReportTemplateId, cancellationToken);
        if (template is null) return Result<ReportTemplate>.Failure(ReportingErrors.ReportTemplateNotFound, ReportingError.ReportTemplateNotFound);
        if (await templateRepository.ExistsByOrganizationIdAndNameAsync(template.OrganizationId, command.Name, template.Id, cancellationToken))
            return Result<ReportTemplate>.Failure(ReportingErrors.DuplicateReportTemplate, ReportingError.DuplicateReportTemplate);

        try
        {
            if (!template.TryUpdate(command))
                return Result<ReportTemplate>.Failure(ReportingErrors.ReportTemplateArchived, ReportingError.ReportTemplateArchived);
            if (template.DefaultForKind) await ClearDefaults(template.OrganizationId, template.ReportKind, template.Id, cancellationToken);
            templateRepository.Update(template);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ReportTemplate>.Success(template);
        }
        catch (ArgumentException ex)
        {
            return Result<ReportTemplate>.Failure(ReportingErrors.InvalidTemplateLayout(ex.Message), ReportingError.InvalidTemplateLayout);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error updating report template {ReportTemplateId}", command.ReportTemplateId);
            return Result<ReportTemplate>.Failure(ReportingErrors.DatabaseError, ReportingError.DatabaseError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating report template {ReportTemplateId}", command.ReportTemplateId);
            return Result<ReportTemplate>.Failure(ReportingErrors.InternalServerError, ReportingError.InternalServerError);
        }
    }

    public async Task<Result<ReportTemplate>> Handle(ChangeReportTemplateStatusCommand command, CancellationToken cancellationToken = default)
    {
        var template = await templateRepository.FindByIdWithSectionsAsync(command.ReportTemplateId, cancellationToken);
        if (template is null) return Result<ReportTemplate>.Failure(ReportingErrors.ReportTemplateNotFound, ReportingError.ReportTemplateNotFound);
        if (!template.TryChangeStatus(command.Status))
            return Result<ReportTemplate>.Failure(ReportingErrors.InvalidTemplateTransition, ReportingError.InvalidTemplateTransition);

        try
        {
            templateRepository.Update(template);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ReportTemplate>.Success(template);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error changing the status of report template {ReportTemplateId}", command.ReportTemplateId);
            return Result<ReportTemplate>.Failure(ReportingErrors.InternalServerError, ReportingError.InternalServerError);
        }
    }

    /// <summary>Idempotent: seeds the standard templates only when the organization has none.</summary>
    public async Task Handle(SeedDefaultReportTemplatesCommand command, CancellationToken cancellationToken = default)
    {
        if (await templateRepository.ExistsByOrganizationIdAsync(command.OrganizationId, cancellationToken)) return;
        foreach (var template in DefaultTemplates(command.OrganizationId, command.OwnerUserId))
            await templateRepository.AddAsync(template, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    private async Task ClearDefaults(int organizationId, ReportKind kind, int? exceptId, CancellationToken cancellationToken)
    {
        foreach (var other in await templateRepository.FindDefaultsByOrganizationIdAndKindAsync(organizationId, kind, cancellationToken))
        {
            if (other.Id == exceptId) continue;
            other.ClearDefault();
            templateRepository.Update(other);
        }
    }

    private static ReportWidget Process(int position, string code, string label, WidgetViewType view, Aggregation aggregation, WidgetWidth width = WidgetWidth.Half) =>
        new(position, VariableSource.Process, code, label, view, aggregation, width);

    private static IEnumerable<ReportTemplate> DefaultTemplates(int organizationId, int ownerUserId)
    {
        yield return new ReportTemplate(new CreateReportTemplateCommand(organizationId, ownerUserId, "Certificado de calidad",
            "Certificado de recubrimiento HVOF por orden de recuperación", ReportKind.QualityCertificate, DefaultPrimaryColor, DefaultSecondaryColor,
            "Certificado de calidad de recubrimiento HVOF", "Documento generado con EdgeWatch", PageSize.A4, PageOrientation.Portrait, true, true,
            [
                new ReportSectionItem("Orden y componente", 1,
                [
                    new ReportWidget(1, VariableSource.Recuperation, "work_order_number", "Orden de trabajo", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(2, VariableSource.Recuperation, "component_serial", "Serie del componente", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(3, VariableSource.Recuperation, "customer_name", "Cliente", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(4, VariableSource.Session, "recipe", "Receta aplicada", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half)
                ]),
                new ReportSectionItem("Parámetros de proceso", 2,
                [
                    Process(1, "fuel_gas_flow", "Flujo de combustible", WidgetViewType.MinMax, Aggregation.Range),
                    Process(2, "oxygen_flow", "Flujo de oxígeno", WidgetViewType.MinMax, Aggregation.Range),
                    Process(3, "powder_feed_rate", "Alimentación de polvo", WidgetViewType.MinMax, Aggregation.Range),
                    Process(4, "coolant_outlet_temp", "Temperatura de refrigerante", WidgetViewType.MinMax, Aggregation.Range)
                ]),
                new ReportSectionItem("Cumplimiento", 3,
                [
                    new ReportWidget(1, VariableSource.Session, "compliance", "Cumplimiento de banda nominal", WidgetViewType.Value, Aggregation.Avg, WidgetWidth.Half),
                    new ReportWidget(2, VariableSource.FaultCases, "cases", "Casos de falla de la sesión", WidgetViewType.Table, Aggregation.Count, WidgetWidth.Full)
                ])
            ]));

        yield return new ReportTemplate(new CreateReportTemplateCommand(organizationId, ownerUserId, "Reporte de sesión",
            "Resumen operativo de una sesión de rociado con tendencias y desviaciones", ReportKind.SessionReport, DefaultPrimaryColor, DefaultSecondaryColor,
            "Reporte de sesión de rociado HVOF", "Documento generado con EdgeWatch", PageSize.A4, PageOrientation.Portrait, false, true,
            [
                new ReportSectionItem("Resumen", 1,
                [
                    new ReportWidget(1, VariableSource.Session, "system", "Sistema HVOF", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(2, VariableSource.Session, "recipe", "Receta", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(3, VariableSource.Session, "duration", "Duración", WidgetViewType.Value, Aggregation.Last, WidgetWidth.Half),
                    new ReportWidget(4, VariableSource.Session, "compliance", "Cumplimiento", WidgetViewType.Value, Aggregation.Avg, WidgetWidth.Half)
                ]),
                new ReportSectionItem("Tendencias", 2,
                [
                    Process(1, "fuel_gas_flow", "Flujo de combustible", WidgetViewType.Trend, Aggregation.Avg, WidgetWidth.Full),
                    Process(2, "oxygen_flow", "Flujo de oxígeno", WidgetViewType.Trend, Aggregation.Avg, WidgetWidth.Full),
                    Process(3, "powder_feed_rate", "Alimentación de polvo", WidgetViewType.Trend, Aggregation.Avg, WidgetWidth.Full)
                ]),
                new ReportSectionItem("Desviaciones", 3,
                [
                    new ReportWidget(1, VariableSource.Session, "deviations", "Lecturas fuera de banda", WidgetViewType.Table, Aggregation.Count, WidgetWidth.Full),
                    new ReportWidget(2, VariableSource.FaultCases, "cases", "Casos de falla", WidgetViewType.Table, Aggregation.Count, WidgetWidth.Full)
                ])
            ]));
    }
}
