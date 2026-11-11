using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Entities;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class ReportTemplateResourceFromEntityAssembler
{
    public static ReportTemplateResource ToResourceFromEntity(ReportTemplate template) =>
        new(template.Id, template.OrganizationId, template.OwnerUserId, template.Name, template.Description, template.ReportKind.ToUpperSnakeCase(),
            template.PrimaryColor, template.SecondaryColor, template.HeaderText, template.FooterText, template.PageSize.ToUpperSnakeCase(),
            template.Orientation.ToUpperSnakeCase(), template.ShowSignatureBlock, template.DefaultForKind, template.Status.ToUpperSnakeCase(),
            template.Sections.OrderBy(s => s.SectionOrder).Select(ToResourceFromEntity).ToList());

    public static ReportSectionResource ToResourceFromEntity(ReportSection section) =>
        new(section.Title, section.SectionOrder, section.Widgets.OrderBy(w => w.Position).Select(ToResourceFromEntity).ToList());

    public static ReportWidgetResource ToResourceFromEntity(ReportWidget widget) =>
        new(widget.Position, widget.VariableSource.ToUpperSnakeCase(), widget.VariableCode, widget.VariableLabel, widget.ViewType.ToUpperSnakeCase(),
            widget.Aggregation.ToUpperSnakeCase(), widget.Width.ToUpperSnakeCase());
}
