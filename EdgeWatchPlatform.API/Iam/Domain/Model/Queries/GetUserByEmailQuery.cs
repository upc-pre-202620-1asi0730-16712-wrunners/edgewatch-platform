using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Queries;

public record GetUserByEmailQuery(EmailAddress Email);
