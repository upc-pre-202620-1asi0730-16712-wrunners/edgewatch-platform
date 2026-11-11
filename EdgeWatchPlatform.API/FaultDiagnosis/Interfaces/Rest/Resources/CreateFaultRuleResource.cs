using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;

public record CreateFaultRuleResource(
    [Required(ErrorMessage = "RequiredParameter")] [RegularExpression("^[a-z][a-z0-9_]{2,39}$", ErrorMessage = "InvalidParameter")] string Parameter,
    [Required(ErrorMessage = "RequiredBand")] [RegularExpression("^(warning|shutdown)$", ErrorMessage = "InvalidBand")] string Band,
    [Required(ErrorMessage = "RequiredProbableCause")] [MaxLength(300)] string ProbableCause,
    [Required(ErrorMessage = "RequiredRecommendedAction")] [MaxLength(300)] string RecommendedAction,
    [Range(0, 86400, ErrorMessage = "InvalidCooldown")] int CooldownSeconds);
