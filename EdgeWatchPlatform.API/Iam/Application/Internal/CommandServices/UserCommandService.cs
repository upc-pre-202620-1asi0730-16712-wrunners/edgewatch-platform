using EdgeWatchPlatform.API.Iam.Application.CommandServices;
using EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;
using EdgeWatchPlatform.API.Iam.Domain.Model;
using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.Errors;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IOrganizationRepository organizationRepository,
    IRoleRepository roleRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    ILogger<UserCommandService> logger) : IUserCommandService
{
    /// <inheritdoc />
    public async Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<User>.Failure(IamErrors.EmailAlreadyTaken, IamError.EmailAlreadyTaken);
        if (await organizationRepository.ExistsByRucAsync(command.Ruc, cancellationToken))
            return Result<User>.Failure(IamErrors.RucAlreadyTaken, IamError.RucAlreadyTaken);

        var adminRole = await roleRepository.FindByNameAsync(Roles.RoleOrgAdmin, cancellationToken);
        if (adminRole is null)
            return Result<User>.Failure(IamErrors.RoleNotFound, IamError.RoleNotFound);

        try
        {
            // Organization and administrator are created in the same unit of work.
            var organization = new Organization(command);
            await organizationRepository.AddAsync(organization, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var user = new User(command.FullName, command.Email, hashingService.HashPassword(command.Password), organization.Id);
            user.AddRole(adminRole);
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error signing up {Email}", command.Email);
            return Result<User>.Failure(IamErrors.DatabaseError, IamError.DatabaseError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error signing up {Email}", command.Email);
            return Result<User>.Failure(IamErrors.InternalServerError, IamError.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
        if (user is null || user.Status != UserStatus.Active || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(IamErrors.InvalidCredentials, IamError.InvalidCredentials);

        var token = tokenService.GenerateToken(user);
        return Result<(User user, string token)>.Success((user, token));
    }

    /// <inheritdoc />
    public async Task<Result<User>> Handle(AssignRolesToUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null) return Result<User>.Failure(IamErrors.UserNotFound, IamError.UserNotFound);

        var roles = (await roleRepository.FindByIdsAsync(command.RoleIds, cancellationToken)).ToList();
        if (roles.Count != command.RoleIds.Distinct().Count())
            return Result<User>.Failure(IamErrors.RoleNotFound, IamError.RoleNotFound);

        try
        {
            user.ReplaceRoles(roles);
            userRepository.Update(user);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error assigning roles to user {UserId}", command.UserId);
            return Result<User>.Failure(IamErrors.InternalServerError, IamError.InternalServerError);
        }
    }
}
