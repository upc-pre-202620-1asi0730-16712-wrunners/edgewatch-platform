using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class ReportTemplateCommandsFromResourceAssembler
{
    public static CreateReportTemplateCommand ToCommandFromResource(int organizationId, int ownerUserId, CreateReportTemplateResource resource) =>
        new(organizationId, ownerUserId, resource.Name, resource.Description ?? string.Empty, resource.ReportKind.ToEnum<ReportKind>(),
            resource.PrimaryColor, resource.SecondaryColor, resource.HeaderText ?? string.Empty, resource.FooterText ?? string.Empty,
            resource.PageSize.ToEnum<PageSize>(), resource.Orientation.ToEnum<PageOrientation>(), resource.ShowSignatureBlock, resource.DefaultForKind,
            resource.Sections.Select(ToItem).ToList());

    public static UpdateReportTemplateCommand ToCommandFromResource(int id, UpdateReportTemplateResource resource) =>
        new(id, resource.Name, resource.Description ?? string.Empty, resource.PrimaryColor, resource.SecondaryColor, resource.HeaderText ?? string.Empty,
            resource.FooterText ?? string.Empty, resource.PageSize.ToEnum<PageSize>(), resource.Orientation.ToEnum<PageOrientation>(),
            resource.ShowSignatureBlock, resource.DefaultForKind, resource.Sections.Select(ToItem).ToList());

    private static ReportSectionItem ToItem(ReportSectionResource section) =>
        new(section.Title, section.SectionOrder, section.Widgets.Select(w => new ReportWidget(w.Position, w.VariableSource.ToEnum<VariableSource>(),
            w.VariableCode, w.VariableLabel, w.ViewType.ToEnum<WidgetViewType>(), w.Aggregation.ToEnum<Aggregation>(), w.Width.ToEnum<WidgetWidth>())).ToList());
}
