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

        var recuperation = builder.Entity<Recuperation>();
        recuperation.HasKey(r => r.Id);
        recuperation.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        recuperation.Property(r => r.SupplierOrganizationId).IsRequired();
        recuperation.Property(r => r.WorkOrderNumber).HasConversion(w => w.Value, v => new WorkOrderNumber(v)).HasMaxLength(30).IsRequired();
        recuperation.Property(r => r.ManufacturingOrderNumber).HasMaxLength(30).IsRequired();
        recuperation.Property(r => r.ComponentId).IsRequired();
        recuperation.Property(r => r.CustomerId).IsRequired();
        recuperation.Property(r => r.Segment).HasMaxLength(40).IsRequired();
        recuperation.Property(r => r.Operation).HasMaxLength(120).IsRequired();
        recuperation.Property(r => r.WeightValue).HasPrecision(10, 3).IsRequired();
        recuperation.Property(r => r.WeightUnitSymbol).HasMaxLength(10).IsRequired();
        recuperation.Property(r => r.HourmeterAtEntry).IsRequired();
        recuperation.Property(r => r.PowderSupplier).HasMaxLength(80).IsRequired();
        recuperation.Property(r => r.PowderLotNumber).HasMaxLength(40).IsRequired();
        recuperation.Property(r => r.PowderChemicalComposition).HasMaxLength(40).IsRequired();
        recuperation.Property(r => r.Status).HasUpperSnakeCaseConversion().IsRequired();
        recuperation.Property(r => r.ReceivedAt).IsRequired();
        recuperation.HasIndex(r => new { r.SupplierOrganizationId, r.WorkOrderNumber }).IsUnique();
        recuperation.HasOne<Customer>().WithMany().HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.Restrict);
        recuperation.HasOne<RecoveredComponent>().WithMany().HasForeignKey(r => r.ComponentId).OnDelete(DeleteBehavior.Restrict);
        recuperation.OwnsMany(r => r.LinkedSessions, linkedSession =>
        {
            linkedSession.ToTable("recuperation_linked_sessions");
            linkedSession.WithOwner().HasForeignKey("RecuperationId");
            linkedSession.Property<int>("Id").ValueGeneratedOnAdd();
            linkedSession.HasKey("Id");
            linkedSession.Property(s => s.SessionId).IsRequired();
            linkedSession.HasIndex("RecuperationId", nameof(LinkedSession.SessionId)).IsUnique();
        });
        recuperation.Navigation(r => r.LinkedSessions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
