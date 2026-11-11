using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Commands;

public record SignInCommand(EmailAddress Email, string Password);
