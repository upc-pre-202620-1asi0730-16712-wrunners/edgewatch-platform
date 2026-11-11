using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record RecipeParameterResource(
    [Required(ErrorMessage = "RequiredParameter")] [RegularExpression("^[a-z][a-z0-9_]{2,39}$", ErrorMessage = "InvalidParameter")] string Parameter,
    decimal Setpoint, decimal LowerShutdown, decimal LowerWarning, decimal NominalMin, decimal NominalMax, decimal UpperWarning, decimal UpperShutdown,
    [Required(ErrorMessage = "RequiredUnitSymbol")] [MaxLength(15)] string UnitSymbol,
    [Required(ErrorMessage = "RequiredUnitCategory")] [RegularExpression("^(flow|mass_flow|pressure|temperature|speed|dimensionless)$", ErrorMessage = "InvalidUnitCategory")] string UnitCategory);
