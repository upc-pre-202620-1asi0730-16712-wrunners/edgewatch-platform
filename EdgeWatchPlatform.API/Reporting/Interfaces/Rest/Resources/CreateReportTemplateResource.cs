using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

/// <summary>Request body of POST /report-templates. The organization and owner come from the token.</summary>
public record CreateReportTemplateResource(
    [Required(ErrorMessage = "RequiredName")] [MaxLength(120)] string Name,
    [MaxLength(300)] string? Description,
    [Required(ErrorMessage = "RequiredReportKind")] [RegularExpression("^(QUALITY_CERTIFICATE|SESSION_REPORT|FAULT_SUMMARY)$", ErrorMessage = "InvalidReportKind")] string ReportKind,
    [Required(ErrorMessage = "RequiredColor")] [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "InvalidColor")] string PrimaryColor,
    [Required(ErrorMessage = "RequiredColor")] [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "InvalidColor")] string SecondaryColor,
    [MaxLength(160)] string? HeaderText,
    [MaxLength(160)] string? FooterText,
    [Required(ErrorMessage = "RequiredPageSize")] [RegularExpression("^(A4|LETTER)$", ErrorMessage = "InvalidPageSize")] string PageSize,
    [Required(ErrorMessage = "RequiredOrientation")] [RegularExpression("^(PORTRAIT|LANDSCAPE)$", ErrorMessage = "InvalidOrientation")] string Orientation,
    bool ShowSignatureBlock,
    bool DefaultForKind,
    [Required] [MinLength(1, ErrorMessage = "RequiredSections")] IReadOnlyCollection<ReportSectionResource> Sections);
