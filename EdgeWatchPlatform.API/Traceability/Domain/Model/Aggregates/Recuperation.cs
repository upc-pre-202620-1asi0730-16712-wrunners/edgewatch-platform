using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;

/// <summary>A recuperation work order: the HVOF recovery of a customer's component.</summary>
public partial class Recuperation
{
    private readonly List<LinkedSession> _linkedSessions = [];

    protected Recuperation()
    {
        WorkOrderNumber = null!;
        ManufacturingOrderNumber = string.Empty;
        Segment = string.Empty;
        Operation = string.Empty;
        WeightUnitSymbol = string.Empty;
        PowderSupplier = string.Empty;
        PowderLotNumber = string.Empty;
        PowderChemicalComposition = string.Empty;
    }

    public Recuperation(OpenRecuperationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        SupplierOrganizationId = command.SupplierOrganizationId;
        WorkOrderNumber = command.WorkOrderNumber;
        ManufacturingOrderNumber = command.ManufacturingOrderNumber.Trim();
        ComponentId = command.ComponentId;
        CustomerId = command.CustomerId;
        Segment = command.Segment.Trim();
        Operation = command.Operation.Trim();
        WeightValue = command.WeightValue;
        WeightUnitSymbol = command.WeightUnitSymbol.Trim();
        HourmeterAtEntry = command.HourmeterAtEntry;
        PowderSupplier = command.PowderSupplier.Trim();
        PowderLotNumber = command.PowderLotNumber.Trim();
        PowderChemicalComposition = command.PowderChemicalComposition.Trim();
        ReceivedAt = command.ReceivedAt;
        Status = RecuperationStatus.Received;
    }

    public int Id { get; private set; }
    public int SupplierOrganizationId { get; private set; }
    public WorkOrderNumber WorkOrderNumber { get; private set; }
    public string ManufacturingOrderNumber { get; private set; }
    public int ComponentId { get; private set; }
    public int CustomerId { get; private set; }
    public string Segment { get; private set; }
    public string Operation { get; private set; }
    public decimal WeightValue { get; private set; }
    public string WeightUnitSymbol { get; private set; }
    public int HourmeterAtEntry { get; private set; }
    public string PowderSupplier { get; private set; }
    public string PowderLotNumber { get; private set; }
    public string PowderChemicalComposition { get; private set; }
    public RecuperationStatus Status { get; private set; }
    public DateOnly ReceivedAt { get; private set; }
    public DateOnly? CompletedAt { get; private set; }
    public IReadOnlyCollection<LinkedSession> LinkedSessions => _linkedSessions.AsReadOnly();

    /// <summary>Updates the editable data. Work order, component, customer and supplier are the identity and never change.</summary>
    public void Update(UpdateRecuperationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ManufacturingOrderNumber = command.ManufacturingOrderNumber.Trim();
        Segment = command.Segment.Trim();
        Operation = command.Operation.Trim();
        WeightValue = command.WeightValue;
        WeightUnitSymbol = command.WeightUnitSymbol.Trim();
        HourmeterAtEntry = command.HourmeterAtEntry;
        PowderSupplier = command.PowderSupplier.Trim();
        PowderLotNumber = command.PowderLotNumber.Trim();
        PowderChemicalComposition = command.PowderChemicalComposition.Trim();
    }

    /// <summary>
    ///     Allowed transitions: RECEIVED→IN_PROGRESS, IN_PROGRESS→COMPLETED, COMPLETED→DELIVERED|REWORK, REWORK→IN_PROGRESS.
    /// </summary>
    /// <returns><c>true</c> when the transition was applied; <c>false</c> when it is not allowed.</returns>
    public bool TryChangeStatus(RecuperationStatus next)
    {
        var allowed = (Status, next) switch
        {
            (RecuperationStatus.Received, RecuperationStatus.InProgress) => true,
            (RecuperationStatus.InProgress, RecuperationStatus.Completed) => true,
            (RecuperationStatus.Completed, RecuperationStatus.Delivered) => true,
            (RecuperationStatus.Completed, RecuperationStatus.Rework) => true,
            (RecuperationStatus.Rework, RecuperationStatus.InProgress) => true,
            _ => false
        };
        if (!allowed) return false;

        Status = next;
        if (next == RecuperationStatus.Completed) CompletedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        else if (next == RecuperationStatus.Rework) CompletedAt = null;
        return true;
    }

    /// <summary>Links a monitoring session (idempotent). A RECEIVED recuperation moves to IN_PROGRESS.</summary>
    public void LinkSession(int sessionId)
    {
        if (_linkedSessions.All(s => s.SessionId != sessionId)) _linkedSessions.Add(new LinkedSession(sessionId));
        if (Status == RecuperationStatus.Received) Status = RecuperationStatus.InProgress;
    }
}
