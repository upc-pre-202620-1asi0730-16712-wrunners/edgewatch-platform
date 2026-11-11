using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

/// <summary>Peruvian tax identifier (RUC) of an organization: 11 digits.</summary>
public sealed partial record Ruc
{
    public Ruc(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !RucRegex().IsMatch(value))
            throw new ArgumentException("Ruc must have 11 digits.", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    [GeneratedRegex("^[0-9]{11}$", RegexOptions.Compiled)]
    private static partial Regex RucRegex();
}
