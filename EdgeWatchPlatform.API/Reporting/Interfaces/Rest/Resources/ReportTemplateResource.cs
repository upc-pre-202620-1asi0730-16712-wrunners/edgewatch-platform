namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

public record ReportTemplateResource(int Id, int OrganizationId, int OwnerUserId, string Name, string Description, string ReportKind, string PrimaryColor,
    string SecondaryColor, string HeaderText, string FooterText, string PageSize, string Orientation, bool ShowSignatureBlock, bool DefaultForKind,
    string Status, IReadOnlyCollection<ReportSectionResource> Sections);
