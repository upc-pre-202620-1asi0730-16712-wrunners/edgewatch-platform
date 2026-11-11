namespace EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

/// <summary>What a template documents. Exposed as QUALITY_CERTIFICATE, SESSION_REPORT or FAULT_SUMMARY.</summary>
public enum ReportKind
{
    QualityCertificate,
    SessionReport,
    FaultSummary
}
