using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.QueryServices;

public class OrganizationQueryService(IOrganizationRepository organizationRepository) : IOrganizationQueryService
{
    public async Task<Organization?> Handle(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default) =>
        await organizationRepository.FindByIdAsync(query.OrganizationId, cancellationToken);
}
