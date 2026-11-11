using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;

/// <summary>A spray run on an HVOF system for a recuperation, monitored against the frozen bands of a recipe.</summary>
public partial class SpraySession : AggregateRoot
{
    private readonly List<SessionRecipeBand> _recipeBands = [];

    protected SpraySession()
    {
        TimeZone = "America/Lima";
    }

    public SpraySession(int organizationId, int hvofSystemId, int recuperationId, int operatorId, int recipeId, int recipeNumber, string timeZone,
        IEnumerable<SessionRecipeBand> recipeBands) : this()
    {
        OrganizationId = organizationId;
        HvofSystemId = hvofSystemId;
        RecuperationId = recuperationId;
        OperatorId = operatorId;
        RecipeId = recipeId;
        RecipeNumber = recipeNumber;
        TimeZone = string.IsNullOrWhiteSpace(timeZone) ? "America/Lima" : timeZone.Trim();
        StartedAt = DateTimeOffset.UtcNow;
        Status = SessionStatus.Active;
        foreach (var band in recipeBands.DistinctBy(b => b.Parameter)) _recipeBands.Add(band);
        if (_recipeBands.Count == 0) throw new ArgumentException("A session needs the bands of an active recipe.", nameof(recipeBands));
        AddDomainEvent(new SpraySessionStartedEvent(Id, HvofSystemId, RecuperationId, RecipeNumber));
    }

    public int Id { get; private set; }
    public int OrganizationId { get; private set; }
    public int HvofSystemId { get; private set; }
    public int RecuperationId { get; private set; }
    public int OperatorId { get; private set; }
    public int RecipeId { get; private set; }
    public int RecipeNumber { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? EndedAt { get; private set; }
    public string TimeZone { get; private set; }
    public SessionStatus Status { get; private set; }
    public AbortReason? AbortReason { get; private set; }
    public IReadOnlyCollection<SessionRecipeBand> RecipeBands => _recipeBands.AsReadOnly();

    public bool IsActive => Status == SessionStatus.Active;

    public SessionRecipeBand? BandFor(string parameter) => _recipeBands.FirstOrDefault(b => b.Parameter == parameter);

    /// <summary>Classifies a value; parameters without a band in the recipe are nominal.</summary>
    public Band Classify(string parameter, decimal value) => BandFor(parameter)?.Classify(value) ?? Band.Nominal;

    /// <summary>Records a deviation so Fault Diagnosis can react when the unit of work is committed.</summary>
    public void RecordDeviation(string parameter, Band band, decimal value, long epochMillis, string unitSymbol)
    {
        if (!band.IsDeviation()) return;
        AddDomainEvent(new OutOfBandReadingDetectedEvent(Id, HvofSystemId, OrganizationId, parameter, band.ToUpperSnakeCase().ToLowerInvariant(),
            value, epochMillis, unitSymbol));
    }

    /// <summary>ACTIVE → COMPLETED. Returns false otherwise.</summary>
    public bool TryComplete()
    {
        if (Status != SessionStatus.Active) return false;
        Status = SessionStatus.Completed;
        EndedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new SpraySessionEndedEvent(Id, RecuperationId, "completed"));
        return true;
    }

    /// <summary>ACTIVE → ABORTED with a reason. Returns false otherwise.</summary>
    public bool TryAbort(AbortReason reason)
    {
        if (Status != SessionStatus.Active) return false;
        Status = SessionStatus.Aborted;
        AbortReason = reason;
        EndedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new SpraySessionEndedEvent(Id, RecuperationId, "aborted"));
        return true;
    }
}
