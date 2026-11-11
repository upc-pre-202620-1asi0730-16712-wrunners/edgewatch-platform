using System.Text.RegularExpressions;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

/// <summary>Email address of a user, stored lower-cased.</summary>
public sealed partial record EmailAddress
{
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !EmailRegex().IsMatch(value.Trim()))
            throw new ArgumentException("EmailAddress must be a valid email.", nameof(value));
        Value = value.Trim().ToLowerInvariant();
    }

    public string Value { get; }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
