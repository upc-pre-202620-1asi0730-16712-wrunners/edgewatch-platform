using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);
    Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> Handle(GetAllUsersByOrganizationIdQuery query, CancellationToken cancellationToken = default);
}
