using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Errors;

public static class ProcessMonitoringErrors
{
    public static readonly Error SpraySessionNotFound =
        Error.NotFound("ProcessMonitoring.SpraySessionNotFound", "The spray session {0} was not found.");

    public static readonly Error HvofSystemNotFound =
        Error.NotFound("ProcessMonitoring.HvofSystemNotFound", "The HVOF system {0} was not found.");

    public static readonly Error ActiveRecipeNotFound =
        Error.Failure("ProcessMonitoring.ActiveRecipeNotFound", "The HVOF system has no active recipe with number {0}.");

    public static readonly Error RecuperationNotFound =
        Error.NotFound("ProcessMonitoring.RecuperationNotFound", "The recuperation {0} was not found.");

    public static readonly Error RecuperationOfAnotherOrganization =
        Error.Failure("ProcessMonitoring.RecuperationOfAnotherOrganization", "The recuperation {0} belongs to another organization than the HVOF system.");

    public static readonly Error SessionNotActive =
        Error.Failure("ProcessMonitoring.SessionNotActive", "The spray session {0} is not active.");

    public static readonly Error InvalidSessionTransition =
        Error.Failure("ProcessMonitoring.InvalidSessionTransition", "The spray session {0} cannot change to the requested status.");

    public static Error InvalidReadings(string reason) =>
        Error.Validation("ProcessMonitoring.InvalidReadings", reason);

    public static readonly Error DatabaseError =
        Error.Unexpected("ProcessMonitoring.DatabaseError", "A database error occurred during the process monitoring operation.");

    public static readonly Error InternalServerError =
        Error.Unexpected("ProcessMonitoring.InternalServerError", "An unexpected error occurred in Process Monitoring.");
}
