using System.ComponentModel.DataAnnotations;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Shared.Interfaces.ASP.Validation;

/// <summary>Validates that a string is one of the UPPER_SNAKE names of <typeparamref name="TEnum"/>.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
public sealed class EnumValueAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
{
    public override bool IsValid(object? value) => value is null || (value is string s && s.IsValidEnum<TEnum>());
}
