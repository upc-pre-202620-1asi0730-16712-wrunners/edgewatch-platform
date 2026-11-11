using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;

/// <summary>Cause-effect rule of an organization: what a deviation of a parameter in a band probably means and what to do.</summary>
public partial class FaultRule : AggregateRoot
{
    protected FaultRule()
    {
        Parameter = string.Empty;
        ProbableCause = string.Empty;
        RecommendedAction = string.Empty;
    }

    public FaultRule(int organizationId, string parameter, FaultBand band, string probableCause, string recommendedAction, int cooldownSeconds) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter);
        OrganizationId = organizationId;
        Parameter = parameter.Trim();
        Band = band;
        Update(probableCause, recommendedAction, cooldownSeconds);
    }

    public FaultRule(CreateFaultRuleCommand command)
        : this(command.OrganizationId, command.Parameter, command.Band, command.ProbableCause, command.RecommendedAction, command.CooldownSeconds)
    {
    }

    public int Id { get; private set; }
    public int OrganizationId { get; private set; }
    public string Parameter { get; private set; }
    public FaultBand Band { get; private set; }
    public string ProbableCause { get; private set; }
    public string RecommendedAction { get; private set; }
    public int CooldownSeconds { get; private set; }

    public FaultRule Update(string probableCause, string recommendedAction, int cooldownSeconds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(probableCause);
        ArgumentException.ThrowIfNullOrWhiteSpace(recommendedAction);
        ArgumentOutOfRangeException.ThrowIfNegative(cooldownSeconds);
        ProbableCause = probableCause.Trim();
        RecommendedAction = recommendedAction.Trim();
        CooldownSeconds = cooldownSeconds;
        return this;
    }

    public FaultRule Update(UpdateFaultRuleCommand command) => Update(command.ProbableCause, command.RecommendedAction, command.CooldownSeconds);
}
