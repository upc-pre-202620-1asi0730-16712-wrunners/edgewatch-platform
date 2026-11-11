using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Errors;

public static class ReportingErrors
{
    public static readonly Error ReportTemplateNotFound = Error.NotFound("Reporting.ReportTemplateNotFound", "The report template {0} was not found.");
    public static readonly Error GeneratedReportNotFound = Error.NotFound("Reporting.GeneratedReportNotFound", "The generated report {0} was not found.");
    public static readonly Error DuplicateReportTemplate = Error.Conflict("Reporting.DuplicateReportTemplate", "A template named {0} already exists in the organization.");
    public static readonly Error ReportTemplateArchived = Error.Failure("Reporting.ReportTemplateArchived", "The report template {0} is archived and cannot be edited or used.");
    public static readonly Error InvalidTemplateTransition = Error.Failure("Reporting.InvalidTemplateTransition", "The report template {0} is already in the requested status.");
    /// <summary>Carries the domain message of the rejected layout; the code is intentionally not localized so the message is shown as is.</summary>
    public static Error InvalidTemplateLayout(string reason) => Error.Validation("Reporting.InvalidTemplateLayout", reason.Replace("{", "{{").Replace("}", "}}"));
    public static readonly Error Forbidden = Error.Forbidden("Reporting.Forbidden", "Only organization administrators and quality engineers manage report templates.");
    public static readonly Error DatabaseError = Error.Unexpected("Reporting.DatabaseError", "A database error occurred during the reporting operation.");
    public static readonly Error InternalServerError = Error.Unexpected("Reporting.InternalServerError", "An unexpected error occurred in Reporting.");
}
