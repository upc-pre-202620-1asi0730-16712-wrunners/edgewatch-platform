using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Entities;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Events;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;

/// <summary>A customizable report layout of an organization: branding, page setup and ordered sections of widgets.</summary>
public partial class ReportTemplate : AggregateRoot
{
    private readonly List<ReportSection> _sections = [];

    protected ReportTemplate()
    {
        Name = string.Empty;
        Description = string.Empty;
        PrimaryColor = "#0E1A2B";
        SecondaryColor = "#D4781C";
        HeaderText = string.Empty;
        FooterText = string.Empty;
    }

    public ReportTemplate(CreateReportTemplateCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        OrganizationId = command.OrganizationId;
        OwnerUserId = command.OwnerUserId;
        ReportKind = command.ReportKind;
        Status = TemplateStatus.Active;
        Apply(command.Name, command.Description, command.PrimaryColor, command.SecondaryColor, command.HeaderText, command.FooterText,
            command.PageSize, command.Orientation, command.ShowSignatureBlock, command.DefaultForKind, command.Sections);
        AddDomainEvent(new ReportTemplateCreatedEvent(Id, OrganizationId, ReportKind.ToUpperSnakeCase()));
    }

    public int Id { get; private set; }
    public int OrganizationId { get; private set; }
    public int OwnerUserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ReportKind ReportKind { get; private set; }
    public string PrimaryColor { get; private set; }
    public string SecondaryColor { get; private set; }
    public string HeaderText { get; private set; }
    public string FooterText { get; private set; }
    public PageSize PageSize { get; private set; }
    public PageOrientation Orientation { get; private set; }
    public bool ShowSignatureBlock { get; private set; }
    public bool DefaultForKind { get; private set; }
    public TemplateStatus Status { get; private set; }
    public IReadOnlyCollection<ReportSection> Sections => _sections.AsReadOnly();

    public bool IsActive => Status == TemplateStatus.Active;

    /// <summary>Replaces the layout. Archived templates are read-only.</summary>
    public bool TryUpdate(UpdateReportTemplateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (Status != TemplateStatus.Active) return false;
        Apply(command.Name, command.Description, command.PrimaryColor, command.SecondaryColor, command.HeaderText, command.FooterText,
            command.PageSize, command.Orientation, command.ShowSignatureBlock, command.DefaultForKind, command.Sections);
        return true;
    }

    /// <summary>ACTIVE ⇄ ARCHIVED. Returns false when the template is already in that status.</summary>
    public bool TryChangeStatus(TemplateStatus status)
    {
        if (Status == status) return false;
        Status = status;
        if (status == TemplateStatus.Archived) DefaultForKind = false;
        return true;
    }

    /// <summary>Used when another template of the same kind becomes the default.</summary>
    public void ClearDefault() => DefaultForKind = false;

    private void Apply(string name, string description, string primaryColor, string secondaryColor, string headerText, string footerText,
        PageSize pageSize, PageOrientation orientation, bool showSignatureBlock, bool defaultForKind, IReadOnlyCollection<ReportSectionItem> sections)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(sections);
        if (sections.Count == 0) throw new ArgumentException("A template needs at least one section.", nameof(sections));
        if (sections.Select(s => s.SectionOrder).Distinct().Count() != sections.Count)
            throw new ArgumentException("Section orders must be unique.", nameof(sections));
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        PrimaryColor = HexColor.Normalize(primaryColor);
        SecondaryColor = HexColor.Normalize(secondaryColor);
        HeaderText = headerText?.Trim() ?? string.Empty;
        FooterText = footerText?.Trim() ?? string.Empty;
        PageSize = pageSize;
        Orientation = orientation;
        ShowSignatureBlock = showSignatureBlock;
        DefaultForKind = defaultForKind;
        _sections.Clear();
        foreach (var item in sections.OrderBy(s => s.SectionOrder)) _sections.Add(new ReportSection(item.Title, item.SectionOrder, item.Widgets));
    }
}
