using EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;
using BCryptNet = BCrypt.Net.BCrypt;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Hashing.BCrypt.Services;

/// <summary>BCrypt implementation of the hashing service.</summary>
public class HashingService : IHashingService
{
    public string HashPassword(string password) => BCryptNet.HashPassword(password);

    public bool VerifyPassword(string password, string passwordHash) => BCryptNet.Verify(password, passwordHash);
}
