using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

/// <summary>Replaces the layout of a template. The organization, owner and kind never change.</summary>
public record UpdateReportTemplateCommand(int ReportTemplateId, string Name, string Description, string PrimaryColor, string SecondaryColor,
    string HeaderText, string FooterText, PageSize PageSize, PageOrientation Orientation, bool ShowSignatureBlock, bool DefaultForKind,
    IReadOnlyCollection<ReportSectionItem> Sections);
