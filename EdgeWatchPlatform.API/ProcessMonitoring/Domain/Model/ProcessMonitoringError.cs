namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model;

/// <summary>Errors of the Process Monitoring bounded context.</summary>
public enum ProcessMonitoringError
{
    None,
    SpraySessionNotFound,
    HvofSystemNotFound,
    ActiveRecipeNotFound,
    RecuperationNotFound,
    RecuperationOfAnotherOrganization,
    SessionNotActive,
    InvalidSessionTransition,
    InvalidReadings,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
