using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;

public record UpdateFaultRuleResource(
    [Required(ErrorMessage = "RequiredProbableCause")] [MaxLength(300)] string ProbableCause,
    [Required(ErrorMessage = "RequiredRecommendedAction")] [MaxLength(300)] string RecommendedAction,
    [Range(0, 86400, ErrorMessage = "InvalidCooldown")] int CooldownSeconds);
