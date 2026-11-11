using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Interfaces.Acl;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Application.Acl;

public class IamContextFacade(IUserQueryService userQueryService, IOrganizationQueryService organizationQueryService) : IIamContextFacade
{
    public async Task<int> FetchOrganizationIdByUserId(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user?.OrganizationId ?? 0;
    }

    public async Task<string> FetchOrganizationTypeByOrganizationId(int organizationId, CancellationToken cancellationToken = default)
    {
        var organization = await organizationQueryService.Handle(new GetOrganizationByIdQuery(organizationId), cancellationToken);
        return organization?.OrganizationType.ToUpperSnakeCase() ?? string.Empty;
    }

    public async Task<bool> ExistsOrganizationById(int organizationId, CancellationToken cancellationToken = default) =>
        await organizationQueryService.Handle(new GetOrganizationByIdQuery(organizationId), cancellationToken) is not null;
}
