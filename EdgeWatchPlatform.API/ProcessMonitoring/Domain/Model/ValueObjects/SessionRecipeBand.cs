namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

/// <summary>
///     Snapshot of one recipe parameter frozen when the session starts, so later recipe changes never reclassify old readings.
/// </summary>
public record SessionRecipeBand(string Parameter, decimal Setpoint, decimal LowerShutdown, decimal LowerWarning, decimal NominalMin,
    decimal NominalMax, decimal UpperWarning, decimal UpperShutdown, string UnitSymbol, string UnitCategory)
{
    public Band Classify(decimal value)
    {
        if (value < LowerShutdown || value > UpperShutdown) return Band.Shutdown;
        if (value < LowerWarning || value > UpperWarning) return Band.Warning;
        if (value < NominalMin || value > NominalMax) return Band.OutOfNominal;
        return Band.Nominal;
    }
}
