using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Errors;

/// <summary>Domain errors of the Equipment bounded context.</summary>
public static class EquipmentErrors
{
    public static readonly Error HvofSystemNotFound =
        Error.NotFound("Equipment.HvofSystemNotFound", "The HVOF system {0} was not found.");

    public static readonly Error DuplicateHvofSystemCode =
        Error.Conflict("Equipment.DuplicateHvofSystemCode", "An HVOF system with code {0} already exists in the organization.");

    public static readonly Error SubsystemNotFound =
        Error.NotFound("Equipment.SubsystemNotFound", "The subsystem {0} was not found in the HVOF system.");

    public static readonly Error DuplicateSubsystemAlias =
        Error.Conflict("Equipment.DuplicateSubsystemAlias", "A subsystem with alias {0} already exists in the HVOF system.");

    public static readonly Error DuplicatePartSerialNumber =
        Error.Conflict("Equipment.DuplicatePartSerialNumber", "A part with serial number {0} already exists in the subsystem.");

    public static readonly Error DuplicateControllerNumber =
        Error.Conflict("Equipment.DuplicateControllerNumber", "Controller number {0} already exists in the HVOF system.");

    public static readonly Error DatabaseError =
        Error.Unexpected("Equipment.DatabaseError", "A database error occurred during the equipment operation.");

    public static readonly Error InternalServerError =
        Error.Unexpected("Equipment.InternalServerError", "An unexpected error occurred in Equipment.");
}
