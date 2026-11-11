using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Name of a process parameter in snake_case (fuel_gas_flow, oxygen_flow, ...).</summary>
public sealed partial record ParameterName
{
    public ParameterName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !NameRegex().IsMatch(value))
            throw new ArgumentException("ParameterName must be snake_case (e.g. fuel_gas_flow).", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    [GeneratedRegex("^[a-z][a-z0-9_]{2,39}$", RegexOptions.Compiled)]
    private static partial Regex NameRegex();
}
