using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;

/// <summary>Generates and validates JWT bearer tokens.</summary>
public interface ITokenService
{
    string GenerateToken(User user);

    /// <summary>Returns the user id carried by a valid token, or null.</summary>
    Task<int?> ValidateToken(string token);
}
