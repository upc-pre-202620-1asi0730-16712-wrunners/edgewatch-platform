using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Events;

public record UserRegisteredEvent(string Email, string FullName, int OrganizationId) : IEvent;
