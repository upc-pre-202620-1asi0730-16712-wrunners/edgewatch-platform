using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class TraceabilityModelConfiguration
{
    public static void ApplyTraceabilityConfiguration(this ModelBuilder builder)
    {
        var customer = builder.Entity<Customer>();
        customer.HasKey(c => c.Id);
        customer.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        customer.Property(c => c.SupplierOrganizationId).IsRequired();
        customer.Property(c => c.LegalName).HasMaxLength(150).IsRequired();
        customer.Property(c => c.Ruc).HasConversion(r => r.Value, v => new Ruc(v)).HasMaxLength(11).IsRequired();
        customer.Property(c => c.MineSite).HasMaxLength(120).IsRequired();
        customer.HasIndex(c => new { c.SupplierOrganizationId, c.Ruc }).IsUnique();

        var component = builder.Entity<RecoveredComponent>();
        component.ToTable("components");
        component.HasKey(c => c.Id);
        component.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        component.Property(c => c.CustomerId).IsRequired();
        component.Property(c => c.SerialNumber).HasMaxLength(60).IsRequired();
        component.Property(c => c.PartNumber).HasMaxLength(40).IsRequired();
        component.Property(c => c.ComponentType).HasUpperSnakeCaseConversion().IsRequired();
        component.Property(c => c.MachineManufacturer).HasMaxLength(80).IsRequired();
        component.Property(c => c.MachineModel).HasMaxLength(80).IsRequired();
        component.Property(c => c.PcrTargetHours).IsRequired();
        component.Property(c => c.Status).HasUpperSnakeCaseConversion().IsRequired();
        component.HasIndex(c => new { c.CustomerId, c.SerialNumber }).IsUnique();
        component.HasOne<Customer>().WithMany().HasForeignKey(c => c.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}
