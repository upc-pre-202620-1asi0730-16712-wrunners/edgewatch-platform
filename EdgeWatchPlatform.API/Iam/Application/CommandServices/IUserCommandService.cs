using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.Iam.Application.CommandServices;

public interface IUserCommandService
{
    /// <summary>Registers the organization and its administrator user.</summary>
    Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);

    /// <summary>Authenticates the credentials and issues a token.</summary>
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken = default);

    Task<Result<User>> Handle(AssignRolesToUserCommand command, CancellationToken cancellationToken = default);
}
