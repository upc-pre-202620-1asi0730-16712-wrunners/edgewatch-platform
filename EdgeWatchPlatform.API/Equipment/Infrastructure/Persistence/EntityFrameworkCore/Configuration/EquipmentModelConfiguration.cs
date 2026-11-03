using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class EquipmentModelConfiguration
{
    public static void ApplyEquipmentConfiguration(this ModelBuilder builder)
    {
        var hvofSystem = builder.Entity<HvofSystem>();
        hvofSystem.HasKey(s => s.Id);
        hvofSystem.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        hvofSystem.Property(s => s.OrganizationId).IsRequired();
        hvofSystem.Property(s => s.Code).HasConversion(c => c.Value, v => new HvofSystemCode(v)).HasMaxLength(20).IsRequired();
        hvofSystem.Property(s => s.SerialNumber).HasMaxLength(60).IsRequired();
        hvofSystem.Property(s => s.SystemManufacturer).HasMaxLength(80).IsRequired();
        hvofSystem.Property(s => s.SystemModel).HasMaxLength(80).IsRequired();
        hvofSystem.Property(s => s.FuelType).HasConversion(f => f.ToString().ToSnakeCase().ToUpperInvariant(), v => Enum.Parse<FuelType>(v.Replace("_", ""), true)).HasMaxLength(20).IsRequired();
        hvofSystem.Property(s => s.Status).HasConversion(st => st.ToString().ToUpperInvariant(), v => Enum.Parse<HvofSystemStatus>(v, true)).HasMaxLength(20).IsRequired();
        hvofSystem.HasIndex(s => new { s.OrganizationId, s.Code }).IsUnique();
    }
}
