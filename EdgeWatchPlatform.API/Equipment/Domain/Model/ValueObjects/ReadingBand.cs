namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Band of a reading against the recipe thresholds, exposed in lower snake case (nominal, out_of_nominal, warning, shutdown).</summary>
public enum ReadingBand { Nominal, OutOfNominal, Warning, Shutdown }
