namespace EdgeWatchPlatform.API.Equipment.Interfaces.Acl;

/// <summary>Thresholds of one recipe parameter, as shared with other bounded contexts (primitives only).</summary>
public record RecipeBandSnapshot(string Parameter, decimal Setpoint, decimal LowerShutdown, decimal LowerWarning, decimal NominalMin,
    decimal NominalMax, decimal UpperWarning, decimal UpperShutdown, string UnitSymbol, string UnitCategory);
