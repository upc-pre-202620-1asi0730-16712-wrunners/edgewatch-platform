using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Events;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;

/// <summary>A deviation that matched a fault rule. Lifecycle: open → acknowledged → resolved.</summary>
public partial class FaultCase : AggregateRoot
{
    protected FaultCase()
    {
        Parameter = string.Empty;
        ProbableCause = string.Empty;
        RecommendedAction = string.Empty;
    }

    public FaultCase(int organizationId, int spraySessionId, int hvofSystemId, string parameter, FaultBand band, decimal value, long epochMillis,
        FaultRule rule) : this()
    {
        ArgumentNullException.ThrowIfNull(rule);
        OrganizationId = organizationId;
        SpraySessionId = spraySessionId;
        HvofSystemId = hvofSystemId;
        Parameter = parameter;
        Band = band;
        Value = value;
        EpochMillis = epochMillis;
        ProbableCause = rule.ProbableCause;
        RecommendedAction = rule.RecommendedAction;
        Status = FaultCaseStatus.Open;
        AddDomainEvent(new FaultCaseOpenedEvent(OrganizationId, SpraySessionId, Parameter, Band.ToString(), ProbableCause));
    }

    public int Id { get; private set; }
    public int OrganizationId { get; private set; }
    public int SpraySessionId { get; private set; }
    public int HvofSystemId { get; private set; }
    public string Parameter { get; private set; }
    public FaultBand Band { get; private set; }
    public decimal Value { get; private set; }
    public long EpochMillis { get; private set; }
    public string ProbableCause { get; private set; }
    public string RecommendedAction { get; private set; }
    public FaultCaseStatus Status { get; private set; }
    public int? AcknowledgedBy { get; private set; }
    public DateTimeOffset? AcknowledgedAt { get; private set; }
    public int? ResolvedBy { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public string? ResolutionNotes { get; private set; }

    /// <summary>open → acknowledged. Returns false otherwise.</summary>
    public bool TryAcknowledge(int userId)
    {
        if (Status != FaultCaseStatus.Open) return false;
        AcknowledgedBy = userId;
        AcknowledgedAt = DateTimeOffset.UtcNow;
        Status = FaultCaseStatus.Acknowledged;
        return true;
    }

    /// <summary>open|acknowledged → resolved (an open case is acknowledged in the same step). Returns false when already resolved.</summary>
    public bool TryResolve(int userId, string? notes)
    {
        if (Status == FaultCaseStatus.Resolved) return false;
        if (Status == FaultCaseStatus.Open) TryAcknowledge(userId);
        ResolvedBy = userId;
        ResolvedAt = DateTimeOffset.UtcNow;
        ResolutionNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        Status = FaultCaseStatus.Resolved;
        return true;
    }
}
