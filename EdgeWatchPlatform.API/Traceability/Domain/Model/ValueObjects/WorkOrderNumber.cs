using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

/// <summary>Work order number of a recuperation (e.g. OT-2026-0451).</summary>
public sealed partial record WorkOrderNumber
{
    public WorkOrderNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !WorkOrderNumberRegex().IsMatch(value))
            throw new ArgumentException("WorkOrderNumber must be 4 to 30 upper case letters, digits or hyphens.", nameof(value));
        Value = value;
    }

    public string Value { get; }
    public override string ToString() => Value;

    [GeneratedRegex("^[A-Z0-9-]{4,30}$", RegexOptions.Compiled)]
    private static partial Regex WorkOrderNumberRegex();
}
