using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record AddSubsystemToHvofSystemCommand(int HvofSystemId, SubsystemType SubsystemType, string Name, string Alias,
    IReadOnlyCollection<ParameterName> Parameters);
