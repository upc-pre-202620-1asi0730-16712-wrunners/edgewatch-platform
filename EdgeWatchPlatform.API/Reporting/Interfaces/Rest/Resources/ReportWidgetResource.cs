using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

/// <summary>A widget of a section. Enumerations travel in UPPER_SNAKE_CASE.</summary>
public record ReportWidgetResource(
    [Range(1, 50, ErrorMessage = "InvalidWidgetPosition")] int Position,
    [Required(ErrorMessage = "RequiredVariableSource")] [RegularExpression("^(PROCESS|SESSION|RECUPERATION|FAULT_CASES)$", ErrorMessage = "InvalidVariableSource")] string VariableSource,
    [Required(ErrorMessage = "RequiredVariableCode")] [RegularExpression("^[a-z][a-z0-9_]{2,39}$", ErrorMessage = "InvalidVariableCode")] string VariableCode,
    [Required(ErrorMessage = "RequiredVariableLabel")] [MaxLength(80)] string VariableLabel,
    [Required(ErrorMessage = "RequiredViewType")] [RegularExpression("^(VALUE|MIN_MAX|TREND|TABLE|TEXT)$", ErrorMessage = "InvalidViewType")] string ViewType,
    [Required(ErrorMessage = "RequiredAggregation")] [RegularExpression("^(AVG|MIN|MAX|RANGE|LAST|COUNT)$", ErrorMessage = "InvalidAggregation")] string Aggregation,
    [Required(ErrorMessage = "RequiredWidth")] [RegularExpression("^(HALF|FULL)$", ErrorMessage = "InvalidWidth")] string Width);
