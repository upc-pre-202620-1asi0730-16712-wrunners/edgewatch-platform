using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class ProcessMonitoringModelConfiguration
{
    public static void ApplyProcessMonitoringConfiguration(this ModelBuilder builder)
    {
        var session = builder.Entity<SpraySession>();
        session.HasKey(s => s.Id);
        session.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        session.Property(s => s.OrganizationId).IsRequired();
        session.Property(s => s.HvofSystemId).IsRequired();
        session.Property(s => s.RecuperationId).IsRequired();
        session.Property(s => s.OperatorId).IsRequired();
        session.Property(s => s.RecipeId).IsRequired();
        session.Property(s => s.RecipeNumber).IsRequired();
        session.Property(s => s.StartedAt).IsRequired();
        session.Property(s => s.TimeZone).HasMaxLength(60).IsRequired();
        session.Property(s => s.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        session.Property(s => s.AbortReason).HasConversion(r => r == null ? null : r.Value.ToUpperSnakeCase(), v => v == null ? null : v.ToEnum<Domain.Model.ValueObjects.AbortReason>()).HasMaxLength(30);
        session.HasIndex(s => new { s.OrganizationId, s.Status });
        session.HasIndex(s => s.RecuperationId);
        session.Ignore(s => s.IsActive);
        session.OwnsMany(s => s.RecipeBands, band =>
        {
            band.ToTable("spray_session_recipe_bands");
            band.WithOwner().HasForeignKey("SpraySessionId");
            band.Property<int>("Id").ValueGeneratedOnAdd();
            band.HasKey("Id");
            band.Property(b => b.Parameter).HasMaxLength(40).IsRequired();
            foreach (var column in new[] { "Setpoint", "LowerShutdown", "LowerWarning", "NominalMin", "NominalMax", "UpperWarning", "UpperShutdown" })
                band.Property<decimal>(column).HasPrecision(12, 4).IsRequired();
            band.Property(b => b.UnitSymbol).HasMaxLength(15).IsRequired();
            band.Property(b => b.UnitCategory).HasMaxLength(20).IsRequired();
        });
        session.Navigation(s => s.RecipeBands).UsePropertyAccessMode(PropertyAccessMode.Field);

        var reading = builder.Entity<ProcessReading>();
        reading.HasKey(r => r.Id);
        reading.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        reading.Property(r => r.SpraySessionId).IsRequired();
        reading.Property(r => r.EpochMillis).IsRequired();
        reading.Property(r => r.PlcClockOffsetMillis).IsRequired();
        reading.Property(r => r.TagPath).HasMaxLength(120);
        reading.Property(r => r.Parameter).HasMaxLength(40).IsRequired();
        reading.Property(r => r.Value).HasPrecision(12, 4).IsRequired();
        reading.Property(r => r.UnitSymbol).HasMaxLength(15);
        reading.Property(r => r.UnitCategory).HasMaxLength(20);
        reading.Property(r => r.Band).HasUpperSnakeCaseConversion(20).IsRequired();
        reading.HasOne<SpraySession>().WithMany().HasForeignKey(r => r.SpraySessionId).OnDelete(DeleteBehavior.Cascade);
        reading.HasIndex(r => new { r.SpraySessionId, r.EpochMillis });
        reading.HasIndex(r => new { r.SpraySessionId, r.Band });
    }
}
