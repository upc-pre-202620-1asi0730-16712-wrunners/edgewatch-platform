using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;

/// <summary><c>acknowledged</c> or <c>resolved</c> (with optional notes).</summary>
public record ChangeFaultCaseStatusResource(
    [Required(ErrorMessage = "RequiredStatus")] [RegularExpression("^(acknowledged|resolved)$", ErrorMessage = "InvalidFaultCaseStatus")] string Status,
    [MaxLength(500)] string? ResolutionNotes);
