using System.Security.Claims;
using System.Text;
using EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Tokens.Jwt.Services;

/// <summary>Issues and validates HMAC-SHA256 signed JWT tokens carrying the user id, email and organization.</summary>
public class TokenService(IOptions<TokenSettings> tokenSettings, ILogger<TokenService> logger) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;

    private byte[] Key => Encoding.UTF8.GetBytes(Environment.ExpandEnvironmentVariables(_tokenSettings.Secret));

    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email.Value),
                new Claim("organizationId", user.OrganizationId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddDays(_tokenSettings.ExpirationDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
        };
        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }

    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        try
        {
            var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });
            if (!result.IsValid) return null;
            var jwtToken = (JsonWebToken)result.SecurityToken;
            return int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Invalid bearer token");
            return null;
        }
    }
}
