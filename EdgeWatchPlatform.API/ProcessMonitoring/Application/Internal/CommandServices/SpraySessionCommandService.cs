using EdgeWatchPlatform.API.ProcessMonitoring.Application.CommandServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.OutboundServices.Acl;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Errors;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.CommandServices;

public class SpraySessionCommandService(
    ISpraySessionRepository sessionRepository,
    IProcessReadingRepository readingRepository,
    ExternalEquipmentService externalEquipmentService,
    ExternalTraceabilityService externalTraceabilityService,
    IUnitOfWork unitOfWork,
    ILogger<SpraySessionCommandService> logger) : ISpraySessionCommandService
{
    /// <summary>Starts a session: validates system, recipe and recuperation through the facades, freezes the recipe bands and links the session.</summary>
    public async Task<Result<SpraySession>> Handle(StartSpraySessionCommand command, CancellationToken cancellationToken = default)
    {
        var organizationId = await externalEquipmentService.FetchHvofSystemOrganizationId(command.HvofSystemId, cancellationToken);
        if (organizationId is null)
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.HvofSystemNotFound, ProcessMonitoringError.HvofSystemNotFound);

        var recipeId = await externalEquipmentService.FetchActiveRecipeId(command.HvofSystemId, command.RecipeNumber, cancellationToken);
        if (recipeId is null)
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.ActiveRecipeNotFound, ProcessMonitoringError.ActiveRecipeNotFound);

        var supplierOrganizationId = await externalTraceabilityService.FetchRecuperationSupplierOrganizationId(command.RecuperationId, cancellationToken);
        if (supplierOrganizationId is null)
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.RecuperationNotFound, ProcessMonitoringError.RecuperationNotFound);
        if (supplierOrganizationId != organizationId)
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.RecuperationOfAnotherOrganization, ProcessMonitoringError.RecuperationOfAnotherOrganization);

        try
        {
            var bands = await externalEquipmentService.FetchActiveRecipeBands(command.HvofSystemId, command.RecipeNumber, cancellationToken);
            var session = new SpraySession(organizationId.Value, command.HvofSystemId, command.RecuperationId, command.OperatorId, recipeId.Value,
                command.RecipeNumber, command.TimeZone, bands);
            await sessionRepository.AddAsync(session, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await externalTraceabilityService.LinkSessionToRecuperation(command.RecuperationId, session.Id, cancellationToken);
            return Result<SpraySession>.Success(session);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error starting a session on HVOF system {HvofSystemId}", command.HvofSystemId);
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.DatabaseError, ProcessMonitoringError.DatabaseError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error starting a session on HVOF system {HvofSystemId}", command.HvofSystemId);
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.InternalServerError, ProcessMonitoringError.InternalServerError);
        }
    }

    public async Task<Result<SpraySession>> Handle(CompleteSpraySessionCommand command, CancellationToken cancellationToken = default)
    {
        var session = await sessionRepository.FindByIdAsync(command.SpraySessionId, cancellationToken);
        if (session is null) return Result<SpraySession>.Failure(ProcessMonitoringErrors.SpraySessionNotFound, ProcessMonitoringError.SpraySessionNotFound);
        if (!session.TryComplete()) return Result<SpraySession>.Failure(ProcessMonitoringErrors.InvalidSessionTransition, ProcessMonitoringError.InvalidSessionTransition);
        return await Persist(session, cancellationToken);
    }

    public async Task<Result<SpraySession>> Handle(AbortSpraySessionCommand command, CancellationToken cancellationToken = default)
    {
        var session = await sessionRepository.FindByIdAsync(command.SpraySessionId, cancellationToken);
        if (session is null) return Result<SpraySession>.Failure(ProcessMonitoringErrors.SpraySessionNotFound, ProcessMonitoringError.SpraySessionNotFound);
        if (!session.TryAbort(command.Reason)) return Result<SpraySession>.Failure(ProcessMonitoringErrors.InvalidSessionTransition, ProcessMonitoringError.InvalidSessionTransition);
        return await Persist(session, cancellationToken);
    }

    /// <summary>Classifies and stores the readings; deviations are recorded on the session so the event is dispatched on commit.</summary>
    public async Task<Result<IReadOnlyCollection<ProcessReading>>> Handle(IngestProcessReadingsCommand command, CancellationToken cancellationToken = default)
    {
        var session = await sessionRepository.FindByIdAsync(command.SpraySessionId, cancellationToken);
        if (session is null)
            return Result<IReadOnlyCollection<ProcessReading>>.Failure(ProcessMonitoringErrors.SpraySessionNotFound, ProcessMonitoringError.SpraySessionNotFound);
        if (!session.IsActive)
            return Result<IReadOnlyCollection<ProcessReading>>.Failure(ProcessMonitoringErrors.SessionNotActive, ProcessMonitoringError.SessionNotActive);

        try
        {
            var readings = new List<ProcessReading>();
            foreach (var item in command.Readings)
            {
                var band = session.Classify(item.Parameter, item.Value);
                var recipeBand = session.BandFor(item.Parameter);
                readings.Add(new ProcessReading(session.Id, item.EpochMillis, item.PlcClockOffsetMillis, item.TagPath, item.Parameter, item.SubsystemId,
                    item.PartId, item.Value, item.UnitSymbol ?? recipeBand?.UnitSymbol ?? string.Empty,
                    item.UnitCategory ?? recipeBand?.UnitCategory ?? string.Empty, band, item.Derived, item.MappingPending));
                session.RecordDeviation(item.Parameter, band, item.Value, item.EpochMillis, item.UnitSymbol ?? recipeBand?.UnitSymbol ?? string.Empty);
            }

            await readingRepository.AddRangeAsync(readings, cancellationToken);
            sessionRepository.Update(session);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<IReadOnlyCollection<ProcessReading>>.Success(readings);
        }
        catch (ArgumentException ex)
        {
            return Result<IReadOnlyCollection<ProcessReading>>.Failure(ProcessMonitoringErrors.InvalidReadings(ex.Message), ProcessMonitoringError.InvalidReadings);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error ingesting readings of session {SpraySessionId}", command.SpraySessionId);
            return Result<IReadOnlyCollection<ProcessReading>>.Failure(ProcessMonitoringErrors.InternalServerError, ProcessMonitoringError.InternalServerError);
        }
    }

    private async Task<Result<SpraySession>> Persist(SpraySession session, CancellationToken cancellationToken)
    {
        try
        {
            sessionRepository.Update(session);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SpraySession>.Success(session);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating session {SpraySessionId}", session.Id);
            return Result<SpraySession>.Failure(ProcessMonitoringErrors.InternalServerError, ProcessMonitoringError.InternalServerError);
        }
    }
}
