using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Errors;

public static class FaultDiagnosisErrors
{
    public static readonly Error FaultRuleNotFound = Error.NotFound("FaultDiagnosis.FaultRuleNotFound", "The fault rule {0} was not found.");
    public static readonly Error FaultCaseNotFound = Error.NotFound("FaultDiagnosis.FaultCaseNotFound", "The fault case {0} was not found.");
    public static readonly Error DuplicateFaultRule = Error.Conflict("FaultDiagnosis.DuplicateFaultRule", "A rule already exists for {0}.");
    public static readonly Error InvalidFaultCaseTransition = Error.Failure("FaultDiagnosis.InvalidFaultCaseTransition", "The fault case {0} cannot change to the requested status.");
    public static readonly Error Forbidden = Error.Forbidden("FaultDiagnosis.Forbidden", "Only organization administrators manage fault rules.");
    public static readonly Error DatabaseError = Error.Unexpected("FaultDiagnosis.DatabaseError", "A database error occurred during the fault diagnosis operation.");
    public static readonly Error InternalServerError = Error.Unexpected("FaultDiagnosis.InternalServerError", "An unexpected error occurred in Fault Diagnosis.");
}
