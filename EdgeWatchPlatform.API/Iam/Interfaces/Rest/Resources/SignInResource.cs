using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

public record SignInResource(
    [Required(ErrorMessage = "RequiredEmail")] [EmailAddress(ErrorMessage = "InvalidEmail")] string Email,
    [Required(ErrorMessage = "RequiredPassword")] string Password);
