namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>
///     The seven thresholds of a parameter. Invariant: lowerShutdown &lt; lowerWarning &lt; nominalMin ≤ setpoint ≤ nominalMax &lt; upperWarning &lt; upperShutdown.
/// </summary>
public record ThresholdBands(decimal Setpoint, decimal LowerShutdown, decimal LowerWarning, decimal NominalMin, decimal NominalMax,
    decimal UpperWarning, decimal UpperShutdown)
{
    public bool IsOrdered =>
        LowerShutdown < LowerWarning && LowerWarning < NominalMin && NominalMin <= Setpoint && Setpoint <= NominalMax &&
        NominalMax < UpperWarning && UpperWarning < UpperShutdown;

    /// <summary>Classifies a value against the thresholds.</summary>
    public ReadingBand Classify(decimal value)
    {
        if (value < LowerShutdown || value > UpperShutdown) return ReadingBand.Shutdown;
        if (value < LowerWarning || value > UpperWarning) return ReadingBand.Warning;
        if (value < NominalMin || value > NominalMax) return ReadingBand.OutOfNominal;
        return ReadingBand.Nominal;
    }
}
