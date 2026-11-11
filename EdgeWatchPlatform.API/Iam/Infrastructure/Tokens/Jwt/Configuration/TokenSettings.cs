namespace EdgeWatchPlatform.API.Iam.Infrastructure.Tokens.Jwt.Configuration;

/// <summary>JWT settings bound from the <c>TokenSettings</c> section (the secret may reference an environment variable).</summary>
public class TokenSettings
{
    public required string Secret { get; set; }
    public int ExpirationDays { get; set; } = 7;
}
