using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record UpdateSubsystemCommand(int HvofSystemId, int SubsystemId, SubsystemType SubsystemType, string Name, string Alias, IReadOnlyCollection<ParameterName> Parameters);
