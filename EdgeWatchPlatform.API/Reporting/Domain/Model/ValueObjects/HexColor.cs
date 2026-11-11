using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

/// <summary>A #RRGGBB color.</summary>
public static partial class HexColor
{
    [GeneratedRegex("^#[0-9A-Fa-f]{6}$")]
    private static partial Regex Pattern();

    public static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var trimmed = value.Trim();
        if (!Pattern().IsMatch(trimmed)) throw new ArgumentException($"'{value}' is not a #RRGGBB color.", nameof(value));
        return trimmed.ToUpperInvariant();
    }
}
