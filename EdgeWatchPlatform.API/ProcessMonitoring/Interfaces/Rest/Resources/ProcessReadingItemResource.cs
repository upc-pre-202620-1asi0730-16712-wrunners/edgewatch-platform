using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

public record ProcessReadingItemResource(
    [Range(1, long.MaxValue, ErrorMessage = "InvalidEpochMillis")] long EpochMillis,
    long PlcClockOffsetMillis,
    [MaxLength(120)] string? TagPath,
    [Required(ErrorMessage = "RequiredParameter")] [RegularExpression("^[a-z][a-z0-9_]{2,39}$", ErrorMessage = "InvalidParameter")] string Parameter,
    int? SubsystemId,
    int? PartId,
    decimal Value,
    [MaxLength(15)] string? UnitSymbol,
    [MaxLength(20)] string? UnitCategory,
    bool Derived,
    bool MappingPending);
