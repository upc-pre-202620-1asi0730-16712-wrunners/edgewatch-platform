using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;

/// <summary>A process parameter of a recipe with its setpoint, thresholds and unit.</summary>
public class RecipeParameter
{
    protected RecipeParameter()
    {
        Parameter = string.Empty;
        UnitSymbol = string.Empty;
    }

    public RecipeParameter(ParameterName parameter, ThresholdBands bands, string unitSymbol, UnitCategory unitCategory) : this()
    {
        ArgumentNullException.ThrowIfNull(bands);
        if (!bands.IsOrdered)
            throw new ArgumentException($"Thresholds of {parameter.Value} must satisfy lowerShutdown < lowerWarning < nominalMin <= setpoint <= nominalMax < upperWarning < upperShutdown.", nameof(bands));
        ArgumentException.ThrowIfNullOrWhiteSpace(unitSymbol);
        Parameter = parameter.Value;
        Setpoint = bands.Setpoint;
        LowerShutdown = bands.LowerShutdown;
        LowerWarning = bands.LowerWarning;
        NominalMin = bands.NominalMin;
        NominalMax = bands.NominalMax;
        UpperWarning = bands.UpperWarning;
        UpperShutdown = bands.UpperShutdown;
        UnitSymbol = unitSymbol.Trim();
        UnitCategory = unitCategory;
    }

    public int Id { get; private set; }
    public int RecipeId { get; private set; }
    public string Parameter { get; private set; }
    public decimal Setpoint { get; private set; }
    public decimal LowerShutdown { get; private set; }
    public decimal LowerWarning { get; private set; }
    public decimal NominalMin { get; private set; }
    public decimal NominalMax { get; private set; }
    public decimal UpperWarning { get; private set; }
    public decimal UpperShutdown { get; private set; }
    public string UnitSymbol { get; private set; }
    public UnitCategory UnitCategory { get; private set; }

    public ThresholdBands Bands => new(Setpoint, LowerShutdown, LowerWarning, NominalMin, NominalMax, UpperWarning, UpperShutdown);

    public ReadingBand Classify(decimal value) => Bands.Classify(value);
}
