namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;

public enum FaultDiagnosisError
{
    None,
    FaultRuleNotFound,
    FaultCaseNotFound,
    DuplicateFaultRule,
    InvalidFaultCaseTransition,
    Forbidden,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
