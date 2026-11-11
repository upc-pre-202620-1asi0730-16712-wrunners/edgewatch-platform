using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Iam.Application.QueryServices;

public interface IOrganizationQueryService
{
    Task<Organization?> Handle(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default);
}
