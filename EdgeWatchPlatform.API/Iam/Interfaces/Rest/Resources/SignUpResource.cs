using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;

/// <summary>Registers an organization and its administrator user.</summary>
public record SignUpResource(
    [Required(ErrorMessage = "RequiredOrganizationName")] [MaxLength(150)] string OrganizationName,
    [Required(ErrorMessage = "RequiredRuc")] [RegularExpression("^[0-9]{11}$", ErrorMessage = "InvalidRuc")] string Ruc,
    [Required(ErrorMessage = "RequiredOrganizationType")] [RegularExpression("^(RECUPERATION_SUPPLIER|ASSET_OWNER)$", ErrorMessage = "InvalidOrganizationType")] string OrganizationType,
    [Required(ErrorMessage = "RequiredFullName")] [MaxLength(120)] string FullName,
    [Required(ErrorMessage = "RequiredEmail")] [EmailAddress(ErrorMessage = "InvalidEmail")] [MaxLength(120)] string Email,
    [Required(ErrorMessage = "RequiredPassword")] [MinLength(8, ErrorMessage = "InvalidPassword")] [MaxLength(72)] string Password,
    string? TimeZone);
