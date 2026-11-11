using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

/// <summary>A section as it travels inside the create and update commands.</summary>
public record ReportSectionItem(string Title, int SectionOrder, IReadOnlyCollection<ReportWidget> Widgets);
