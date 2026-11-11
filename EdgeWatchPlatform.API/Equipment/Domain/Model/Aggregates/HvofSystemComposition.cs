using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;

/// <summary>Composition of the HVOF system: subsystems (with parts) and controllers.</summary>
public partial class HvofSystem
{
    private readonly List<HvofSubsystem> _subsystems = [];
    private readonly List<Controller> _controllers = [];

    public IReadOnlyCollection<HvofSubsystem> Subsystems => _subsystems.AsReadOnly();
    public IReadOnlyCollection<Controller> Controllers => _controllers.AsReadOnly();

    public bool HasSubsystemWithAlias(string alias) =>
        _subsystems.Any(s => string.Equals(s.Alias, alias.Trim(), StringComparison.OrdinalIgnoreCase));

    public bool HasControllerWithNumber(int controllerNumber) => _controllers.Any(c => c.ControllerNumber == controllerNumber);

    public HvofSubsystem? FindSubsystemById(int subsystemId) => _subsystems.FirstOrDefault(s => s.Id == subsystemId);

    /// <summary>Adds a subsystem. Returns false when the alias is already used in this system.</summary>
    public bool TryAddSubsystem(HvofSubsystem subsystem)
    {
        if (HasSubsystemWithAlias(subsystem.Alias)) return false;
        _subsystems.Add(subsystem);
        return true;
    }

    /// <summary>Adds a controller. Returns false when the controller number is already used in this system.</summary>
    public bool TryAddController(Controller controller)
    {
        if (HasControllerWithNumber(controller.ControllerNumber)) return false;
        _controllers.Add(controller);
        return true;
    }

    public void RecordSubsystemAdded(HvofSubsystem subsystem) =>
        AddDomainEvent(new SubsystemAddedEvent(Id, subsystem.Alias, subsystem.SubsystemType.ToUpperSnakeCase()));

    public void RecordControllerAdded(Controller controller) =>
        AddDomainEvent(new ControllerAddedEvent(Id, controller.ControllerNumber, controller.IpAddress));
}
