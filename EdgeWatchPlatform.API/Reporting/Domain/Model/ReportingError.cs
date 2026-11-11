namespace EdgeWatchPlatform.API.Reporting.Domain.Model;

public enum ReportingError
{
    None,
    ReportTemplateNotFound,
    GeneratedReportNotFound,
    DuplicateReportTemplate,
    ReportTemplateArchived,
    InvalidTemplateTransition,
    InvalidTemplateLayout,
    Forbidden,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
