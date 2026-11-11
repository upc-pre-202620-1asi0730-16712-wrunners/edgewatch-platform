using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Entities;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class ReportingModelConfiguration
{
    public static void ApplyReportingConfiguration(this ModelBuilder builder)
    {
        var template = builder.Entity<ReportTemplate>();
        template.HasKey(t => t.Id);
        template.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        template.Property(t => t.OrganizationId).IsRequired();
        template.Property(t => t.OwnerUserId).IsRequired();
        template.Property(t => t.Name).HasMaxLength(120).IsRequired();
        template.Property(t => t.Description).HasMaxLength(300).IsRequired();
        template.Property(t => t.ReportKind).HasUpperSnakeCaseConversion(30).IsRequired();
        template.Property(t => t.PrimaryColor).HasMaxLength(7).IsRequired();
        template.Property(t => t.SecondaryColor).HasMaxLength(7).IsRequired();
        template.Property(t => t.HeaderText).HasMaxLength(160).IsRequired();
        template.Property(t => t.FooterText).HasMaxLength(160).IsRequired();
        template.Property(t => t.PageSize).HasUpperSnakeCaseConversion(10).IsRequired();
        template.Property(t => t.Orientation).HasUpperSnakeCaseConversion(10).IsRequired();
        template.Property(t => t.ShowSignatureBlock).IsRequired();
        template.Property(t => t.DefaultForKind).IsRequired();
        template.Property(t => t.Status).HasUpperSnakeCaseConversion(10).IsRequired();
        template.HasIndex(t => new { t.OrganizationId, t.Name }).IsUnique();
        template.HasIndex(t => new { t.OrganizationId, t.ReportKind, t.Status });
        template.Ignore(t => t.IsActive);
        template.HasMany(t => t.Sections).WithOne().HasForeignKey(s => s.ReportTemplateId).OnDelete(DeleteBehavior.Cascade);
        template.Navigation(t => t.Sections).UsePropertyAccessMode(PropertyAccessMode.Field);

        var section = builder.Entity<ReportSection>();
        section.HasKey(s => s.Id);
        section.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        section.Property(s => s.ReportTemplateId).IsRequired();
        section.Property(s => s.Title).HasMaxLength(80).IsRequired();
        section.Property(s => s.SectionOrder).IsRequired();
        section.HasIndex(s => new { s.ReportTemplateId, s.SectionOrder }).IsUnique();
        section.OwnsMany(s => s.Widgets, widget =>
        {
            widget.ToTable("report_widgets");
            widget.WithOwner().HasForeignKey("ReportSectionId");
            widget.Property<int>("Id").ValueGeneratedOnAdd();
            widget.HasKey("Id");
            widget.Property(w => w.Position).IsRequired();
            widget.Property(w => w.VariableSource).HasUpperSnakeCaseConversion(20).IsRequired();
            widget.Property(w => w.VariableCode).HasMaxLength(40).IsRequired();
            widget.Property(w => w.VariableLabel).HasMaxLength(80).IsRequired();
            widget.Property(w => w.ViewType).HasUpperSnakeCaseConversion(10).IsRequired();
            widget.Property(w => w.Aggregation).HasUpperSnakeCaseConversion(10).IsRequired();
            widget.Property(w => w.Width).HasUpperSnakeCaseConversion(10).IsRequired();
        });
        section.Navigation(s => s.Widgets).UsePropertyAccessMode(PropertyAccessMode.Field);

        var report = builder.Entity<GeneratedReport>();
        report.HasKey(r => r.Id);
        report.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        report.Property(r => r.ReportTemplateId).IsRequired();
        report.Property(r => r.OrganizationId).IsRequired();
        report.Property(r => r.GeneratedByUserId).IsRequired();
        report.Property(r => r.SubjectType).HasUpperSnakeCaseConversion(20).IsRequired();
        report.Property(r => r.SubjectId).IsRequired();
        report.Property(r => r.Format).HasMaxLength(10).IsRequired();
        report.Property(r => r.FileName).HasMaxLength(160).IsRequired();
        // The MySQL provider reads DATE columns as DateTime, so DateOnly is stored through an explicit conversion.
        report.Property(r => r.PeriodFrom).HasConversion(d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
            d => d.HasValue ? DateOnly.FromDateTime(d.Value) : (DateOnly?)null).HasColumnType("date");
        report.Property(r => r.PeriodTo).HasConversion(d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
            d => d.HasValue ? DateOnly.FromDateTime(d.Value) : (DateOnly?)null).HasColumnType("date");
        report.Property(r => r.GeneratedAt).IsRequired();
        report.HasOne<ReportTemplate>().WithMany().HasForeignKey(r => r.ReportTemplateId).OnDelete(DeleteBehavior.Restrict);
        report.HasIndex(r => new { r.OrganizationId, r.SubjectType, r.SubjectId });
    }
}
