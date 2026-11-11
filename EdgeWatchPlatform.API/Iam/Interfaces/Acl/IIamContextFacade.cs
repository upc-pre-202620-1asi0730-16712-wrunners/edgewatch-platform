namespace EdgeWatchPlatform.API.Iam.Interfaces.Acl;

/// <summary>Anti-corruption layer of IAM: what other bounded contexts may ask about users and organizations.</summary>
public interface IIamContextFacade
{
    /// <summary>Organization id of a user, or 0 when the user does not exist.</summary>
    Task<int> FetchOrganizationIdByUserId(int userId, CancellationToken cancellationToken = default);

    /// <summary>Organization type (RECUPERATION_SUPPLIER / ASSET_OWNER), or empty when the organization does not exist.</summary>
    Task<string> FetchOrganizationTypeByOrganizationId(int organizationId, CancellationToken cancellationToken = default);

    Task<bool> ExistsOrganizationById(int organizationId, CancellationToken cancellationToken = default);
}
