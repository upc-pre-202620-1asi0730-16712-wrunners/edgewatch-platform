using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class FaultDiagnosisModelConfiguration
{
    public static void ApplyFaultDiagnosisConfiguration(this ModelBuilder builder)
    {
        var rule = builder.Entity<FaultRule>();
        rule.HasKey(r => r.Id);
        rule.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        rule.Property(r => r.OrganizationId).IsRequired();
        rule.Property(r => r.Parameter).HasMaxLength(40).IsRequired();
        rule.Property(r => r.Band).HasUpperSnakeCaseConversion(20).IsRequired();
        rule.Property(r => r.ProbableCause).HasMaxLength(300).IsRequired();
        rule.Property(r => r.RecommendedAction).HasMaxLength(300).IsRequired();
        rule.Property(r => r.CooldownSeconds).IsRequired();
        rule.HasIndex(r => new { r.OrganizationId, r.Parameter, r.Band }).IsUnique();

        var faultCase = builder.Entity<FaultCase>();
        faultCase.HasKey(c => c.Id);
        faultCase.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        faultCase.Property(c => c.OrganizationId).IsRequired();
        faultCase.Property(c => c.SpraySessionId).IsRequired();
        faultCase.Property(c => c.HvofSystemId).IsRequired();
        faultCase.Property(c => c.Parameter).HasMaxLength(40).IsRequired();
        faultCase.Property(c => c.Band).HasUpperSnakeCaseConversion(20).IsRequired();
        faultCase.Property(c => c.Value).HasPrecision(12, 4).IsRequired();
        faultCase.Property(c => c.EpochMillis).IsRequired();
        faultCase.Property(c => c.ProbableCause).HasMaxLength(300).IsRequired();
        faultCase.Property(c => c.RecommendedAction).HasMaxLength(300).IsRequired();
        faultCase.Property(c => c.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        faultCase.Property(c => c.ResolutionNotes).HasMaxLength(500);
        faultCase.HasIndex(c => new { c.OrganizationId, c.Status });
        faultCase.HasIndex(c => new { c.SpraySessionId, c.Parameter, c.Band, c.EpochMillis });
    }
}
