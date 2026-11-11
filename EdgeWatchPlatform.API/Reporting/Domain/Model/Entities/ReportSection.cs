using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Entities;

/// <summary>An ordered block of a template with its widgets.</summary>
public class ReportSection
{
    private readonly List<ReportWidget> _widgets = [];

    protected ReportSection()
    {
        Title = string.Empty;
    }

    public ReportSection(string title, int sectionOrder, IEnumerable<ReportWidget> widgets) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentOutOfRangeException.ThrowIfLessThan(sectionOrder, 1);
        Title = title.Trim();
        SectionOrder = sectionOrder;
        foreach (var widget in widgets.OrderBy(w => w.Position)) _widgets.Add(widget.Validated());
        if (_widgets.Select(w => w.Position).Distinct().Count() != _widgets.Count)
            throw new ArgumentException("Widget positions must be unique within a section.", nameof(widgets));
    }

    public int Id { get; private set; }
    public int ReportTemplateId { get; private set; }
    public string Title { get; private set; }
    public int SectionOrder { get; private set; }
    public IReadOnlyCollection<ReportWidget> Widgets => _widgets.AsReadOnly();
}
