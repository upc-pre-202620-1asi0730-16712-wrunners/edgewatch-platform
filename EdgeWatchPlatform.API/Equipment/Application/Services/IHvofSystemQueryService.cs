using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IHvofSystemQueryService
{
    Task<HvofSystem?> Handle(GetHvofSystemByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<HvofSystem>> Handle(GetAllHvofSystemsByOrganizationIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Subsystems of a system, or null when the system does not exist.</summary>
    Task<IEnumerable<HvofSubsystem>?> Handle(GetAllSubsystemsByHvofSystemIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Controllers of a system, or null when the system does not exist.</summary>
    Task<IEnumerable<Controller>?> Handle(GetAllControllersByHvofSystemIdQuery query, CancellationToken cancellationToken = default);
}
