using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Events;

public record OrganizationRegisteredEvent(string Name, string Ruc, string OrganizationType) : IEvent;
