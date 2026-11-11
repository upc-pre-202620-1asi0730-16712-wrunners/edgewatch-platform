using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Errors;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.CommandServices;

public class FaultRuleCommandService(IFaultRuleRepository ruleRepository, IUnitOfWork unitOfWork, ILogger<FaultRuleCommandService> logger) : IFaultRuleCommandService
{
    private const int DefaultCooldownSeconds = 60;

    /// <summary>FESA defaults: parameter, band, probable cause, recommended action.</summary>
    private static readonly (string Parameter, FaultBand Band, string Cause, string Action)[] DefaultRules =
    [
        ("fuel_gas_flow", FaultBand.Shutdown, "Fuel line pressure drop or regulator failure", "Stop spraying; inspect fuel regulator and supply pressure"),
        ("fuel_gas_flow", FaultBand.Warning, "Partial obstruction or drifting mass flow controller", "Check MFC calibration before next pass"),
        ("oxygen_flow", FaultBand.Shutdown, "Oxygen supply interruption", "Stop spraying; verify O2 manifold and valve"),
        ("oxygen_flow", FaultBand.Warning, "O2/fuel ratio drifting", "Review combustion ratio setpoint"),
        ("powder_feed_rate", FaultBand.Shutdown, "Hopper empty or feed line clogged", "Stop; refill hopper / clear feed line"),
        ("powder_feed_rate", FaultBand.Warning, "Powder humidity or carrier gas fluctuation", "Check powder lot and carrier gas flow"),
        ("carrier_gas_flow", FaultBand.Warning, "Carrier gas leak or regulator drift", "Inspect carrier gas line fittings"),
        ("coolant_outlet_temp", FaultBand.Shutdown, "Coolant flow loss or chiller failure", "Stop; check chiller and coolant pump"),
        ("coolant_outlet_temp", FaultBand.Warning, "Reduced coolant flow", "Clean coolant filter; verify flow"),
        ("coolant_flow", FaultBand.Shutdown, "Coolant pump failure", "Stop; inspect pump and hoses"),
        ("hopper_pressure", FaultBand.Warning, "Hopper seal leak", "Inspect hopper lid seal"),
        ("spindle_speed", FaultBand.Warning, "Spindle drive load variation", "Check part balance and chuck")
    ];

    public async Task<Result<FaultRule>> Handle(CreateFaultRuleCommand command, CancellationToken cancellationToken = default)
    {
        if (await ruleRepository.FindByOrganizationIdAndParameterAndBandAsync(command.OrganizationId, command.Parameter, command.Band, cancellationToken) is not null)
            return Result<FaultRule>.Failure(FaultDiagnosisErrors.DuplicateFaultRule, FaultDiagnosisError.DuplicateFaultRule);

        try
        {
            var rule = new FaultRule(command);
            await ruleRepository.AddAsync(rule, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<FaultRule>.Success(rule);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating fault rule {Parameter}/{Band}", command.Parameter, command.Band);
            return Result<FaultRule>.Failure(FaultDiagnosisErrors.InternalServerError, FaultDiagnosisError.InternalServerError);
        }
    }

    public async Task<Result<FaultRule>> Handle(UpdateFaultRuleCommand command, CancellationToken cancellationToken = default)
    {
        var rule = await ruleRepository.FindByIdAsync(command.FaultRuleId, cancellationToken);
        if (rule is null) return Result<FaultRule>.Failure(FaultDiagnosisErrors.FaultRuleNotFound, FaultDiagnosisError.FaultRuleNotFound);

        try
        {
            rule.Update(command);
            ruleRepository.Update(rule);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<FaultRule>.Success(rule);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating fault rule {FaultRuleId}", command.FaultRuleId);
            return Result<FaultRule>.Failure(FaultDiagnosisErrors.InternalServerError, FaultDiagnosisError.InternalServerError);
        }
    }

    /// <summary>Idempotent: seeds the defaults only when the organization has no rules yet.</summary>
    public async Task Handle(SeedDefaultFaultRulesCommand command, CancellationToken cancellationToken = default)
    {
        if (await ruleRepository.ExistsByOrganizationIdAsync(command.OrganizationId, cancellationToken)) return;
        foreach (var (parameter, band, cause, action) in DefaultRules)
            await ruleRepository.AddAsync(new FaultRule(command.OrganizationId, parameter, band, cause, action, DefaultCooldownSeconds), cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
