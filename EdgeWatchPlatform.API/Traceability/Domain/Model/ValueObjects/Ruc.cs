using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

/// <summary>Peruvian tax id (11 digits).</summary>
public sealed partial record Ruc
{
    public Ruc(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !RucRegex().IsMatch(value))
            throw new ArgumentException("Ruc must be exactly 11 digits.", nameof(value));
        Value = value;
    }

    public string Value { get; }
    public override string ToString() => Value;

    [GeneratedRegex(@"^\d{11}$", RegexOptions.Compiled)]
    private static partial Regex RucRegex();
}
