namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

/// <summary>Band of a reading against the frozen recipe thresholds, exposed in lower snake case.</summary>
public enum Band { Nominal, OutOfNominal, Warning, Shutdown }

public static class BandExtensions
{
    /// <summary>Warning and shutdown readings are deviations.</summary>
    public static bool IsDeviation(this Band band) => band is Band.Warning or Band.Shutdown;
}
