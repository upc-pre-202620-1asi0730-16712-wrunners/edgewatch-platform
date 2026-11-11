using EdgeWatchPlatform.API.Iam.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.EventHandlers;

public class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger) : IEventHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("User {Email} ({FullName}) registered for organization {OrganizationId}",
            domainEvent.Email, domainEvent.FullName, domainEvent.OrganizationId);
        return Task.CompletedTask;
    }
}
