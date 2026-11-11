using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

public record ReportSectionResource(
    [Required(ErrorMessage = "RequiredSectionTitle")] [MaxLength(80)] string Title,
    [Range(1, 50, ErrorMessage = "InvalidSectionOrder")] int SectionOrder,
    [Required] [MinLength(1, ErrorMessage = "RequiredWidgets")] IReadOnlyCollection<ReportWidgetResource> Widgets);
