using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.EventHandlers;

/// <summary>
///     Reacts to deviations detected by Process Monitoring: opens a fault case when a rule matches.
///     The event is dispatched while the readings are being saved, so the case joins the same unit of work.
/// </summary>
public class OutOfBandReadingDetectedEventHandler(IFaultCaseCommandService faultCaseCommandService, ILogger<OutOfBandReadingDetectedEventHandler> logger)
    : IEventHandler<OutOfBandReadingDetectedEvent>
{
    public async Task Handle(OutOfBandReadingDetectedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!domainEvent.Band.IsValidEnum<FaultBand>()) return;
        try
        {
            var faultCase = await faultCaseCommandService.Handle(new OpenFaultCaseCommand(domainEvent.OrganizationId, domainEvent.SpraySessionId,
                domainEvent.HvofSystemId, domainEvent.Parameter, domainEvent.Band.ToEnum<FaultBand>(), domainEvent.Value, domainEvent.EpochMillis), cancellationToken);
            if (faultCase is not null)
                logger.LogInformation("Fault case opened: session {SpraySessionId} {Parameter} {Band} = {Value} {UnitSymbol} ({ProbableCause})",
                    domainEvent.SpraySessionId, domainEvent.Parameter, domainEvent.Band, domainEvent.Value, domainEvent.UnitSymbol, faultCase.ProbableCause);
        }
        catch (Exception ex)
        {
            // Diagnosis must never break the readings ingestion.
            logger.LogError(ex, "Fault diagnosis failed for session {SpraySessionId} {Parameter}", domainEvent.SpraySessionId, domainEvent.Parameter);
        }
    }
}
