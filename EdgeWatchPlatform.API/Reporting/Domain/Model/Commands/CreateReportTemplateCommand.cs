using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

public record CreateReportTemplateCommand(int OrganizationId, int OwnerUserId, string Name, string Description, ReportKind ReportKind,
    string PrimaryColor, string SecondaryColor, string HeaderText, string FooterText, PageSize PageSize, PageOrientation Orientation,
    bool ShowSignatureBlock, bool DefaultForKind, IReadOnlyCollection<ReportSectionItem> Sections);
