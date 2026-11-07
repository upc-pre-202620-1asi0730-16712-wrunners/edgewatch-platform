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
    }
}
