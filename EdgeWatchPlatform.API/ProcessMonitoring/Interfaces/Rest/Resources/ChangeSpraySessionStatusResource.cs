using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

/// <summary>Ends a session: <c>completed</c>, or <c>aborted</c> with an abort reason.</summary>
public record ChangeSpraySessionStatusResource(
    [Required(ErrorMessage = "RequiredStatus")] [RegularExpression("^(completed|aborted)$", ErrorMessage = "InvalidSessionStatus")] string Status,
    [RegularExpression("^(FLAME_OUT|POWDER_DEPLETED|MACHINE_FAULT|OPERATOR_DECISION|OTHER)$", ErrorMessage = "InvalidAbortReason")] string? AbortReason);
