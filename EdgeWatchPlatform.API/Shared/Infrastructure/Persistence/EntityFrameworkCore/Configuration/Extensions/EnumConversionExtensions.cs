using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Conversions between enums and their UPPER_SNAKE text representation (NATURAL_GAS).</summary>
public static class EnumConversionExtensions
{
    /// <summary>Stores an enum as UPPER_SNAKE text so the API contract matches the web application.</summary>
    public static PropertyBuilder<TEnum> HasUpperSnakeCaseConversion<TEnum>(this PropertyBuilder<TEnum> builder, int maxLength = 30) where TEnum : struct, Enum =>
        builder.HasConversion(e => e.ToUpperSnakeCase(), v => v.ToEnum<TEnum>()).HasMaxLength(maxLength);

    public static string ToUpperSnakeCase<TEnum>(this TEnum value) where TEnum : struct, Enum =>
        value.ToString().ToSnakeCase().ToUpperInvariant();

    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum =>
        Enum.Parse<TEnum>(value.Replace("_", string.Empty), ignoreCase: true);

    public static bool IsValidEnum<TEnum>(this string? value) where TEnum : struct, Enum =>
        value is not null && Enum.TryParse<TEnum>(value.Replace("_", string.Empty), ignoreCase: true, out var parsed)
                          && Enum.IsDefined(parsed);
}
