using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.FindByIdAsync(query.UserId, cancellationToken);

    public async Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.FindByEmailAsync(query.Email, cancellationToken);

    public async Task<IEnumerable<User>> Handle(GetAllUsersByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.FindByOrganizationIdAsync(query.OrganizationId, cancellationToken);
}
