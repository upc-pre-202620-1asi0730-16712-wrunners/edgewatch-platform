namespace EdgeWatchPlatform.API.Equipment.Domain.Model;

/// <summary>Errors of the Equipment bounded context, mapped to HTTP status codes by <c>EquipmentActionResultAssembler</c>.</summary>
public enum EquipmentError
{
    None,
    HvofSystemNotFound,
    DuplicateHvofSystemCode,
    SubsystemNotFound,
    DuplicateSubsystemAlias,
    DuplicatePartSerialNumber,
    DuplicateControllerNumber,
    RecipeNotFound,
    DuplicateRecipeNumber,
    InvalidRecipeTransition,
    InvalidRecipeParameters,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
