using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Short identifier of an HVOF system inside an organization (e.g. HVOF-01).</summary>
public sealed partial record HvofSystemCode
{
    public HvofSystemCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !CodeRegex().IsMatch(value))
            throw new ArgumentException("HvofSystemCode must be 3 to 20 upper case letters, digits or hyphens.", nameof(value));
        Value = value;
    }

    public string Value { get; }
    public override string ToString() => Value;

    [GeneratedRegex("^[A-Z0-9-]{3,20}$", RegexOptions.Compiled)]
    private static partial Regex CodeRegex();
}
