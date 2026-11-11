using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Errors;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.CommandServices;

public class FaultCaseCommandService(
    IFaultCaseRepository caseRepository,
    IFaultRuleRepository ruleRepository,
    IUnitOfWork unitOfWork,
    ILogger<FaultCaseCommandService> logger) : IFaultCaseCommandService
{
    /// <inheritdoc />
    public async Task<FaultCase?> Handle(OpenFaultCaseCommand command, CancellationToken cancellationToken = default)
    {
        var rule = await ruleRepository.FindByOrganizationIdAndParameterAndBandAsync(command.OrganizationId, command.Parameter, command.Band, cancellationToken);
        if (rule is null) return null;

        var latest = await caseRepository.FindLatestBySpraySessionIdAndParameterAndBandAsync(command.SpraySessionId, command.Parameter, command.Band, cancellationToken);
        if (latest is not null && command.EpochMillis - latest.EpochMillis < rule.CooldownSeconds * 1000L) return null;

        var faultCase = new FaultCase(command.OrganizationId, command.SpraySessionId, command.HvofSystemId, command.Parameter, command.Band,
            command.Value, command.EpochMillis, rule);
        await caseRepository.AddAsync(faultCase, cancellationToken);
        return faultCase;
    }

    public async Task<Result<FaultCase>> Handle(AcknowledgeFaultCaseCommand command, CancellationToken cancellationToken = default)
    {
        var faultCase = await caseRepository.FindByIdAsync(command.FaultCaseId, cancellationToken);
        if (faultCase is null) return Result<FaultCase>.Failure(FaultDiagnosisErrors.FaultCaseNotFound, FaultDiagnosisError.FaultCaseNotFound);
        if (!faultCase.TryAcknowledge(command.UserId))
            return Result<FaultCase>.Failure(FaultDiagnosisErrors.InvalidFaultCaseTransition, FaultDiagnosisError.InvalidFaultCaseTransition);
        return await Persist(faultCase, cancellationToken);
    }

    public async Task<Result<FaultCase>> Handle(ResolveFaultCaseCommand command, CancellationToken cancellationToken = default)
    {
        var faultCase = await caseRepository.FindByIdAsync(command.FaultCaseId, cancellationToken);
        if (faultCase is null) return Result<FaultCase>.Failure(FaultDiagnosisErrors.FaultCaseNotFound, FaultDiagnosisError.FaultCaseNotFound);
        if (!faultCase.TryResolve(command.UserId, command.ResolutionNotes))
            return Result<FaultCase>.Failure(FaultDiagnosisErrors.InvalidFaultCaseTransition, FaultDiagnosisError.InvalidFaultCaseTransition);
        return await Persist(faultCase, cancellationToken);
    }

    private async Task<Result<FaultCase>> Persist(FaultCase faultCase, CancellationToken cancellationToken)
    {
        try
        {
            caseRepository.Update(faultCase);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<FaultCase>.Success(faultCase);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating fault case {FaultCaseId}", faultCase.Id);
            return Result<FaultCase>.Failure(FaultDiagnosisErrors.InternalServerError, FaultDiagnosisError.InternalServerError);
        }
    }
}
