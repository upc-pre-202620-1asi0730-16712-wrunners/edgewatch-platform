using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource) => new(new EmailAddress(resource.Email), resource.Password);
}
