using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
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
        hvofSystem.Property(s => s.FuelType).HasUpperSnakeCaseConversion(20).IsRequired();
        hvofSystem.Property(s => s.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        hvofSystem.HasIndex(s => new { s.OrganizationId, s.Code }).IsUnique();
        hvofSystem.HasMany(s => s.Subsystems).WithOne().HasForeignKey(sub => sub.HvofSystemId).OnDelete(DeleteBehavior.Cascade);
        hvofSystem.HasMany(s => s.Controllers).WithOne().HasForeignKey(c => c.HvofSystemId).OnDelete(DeleteBehavior.Cascade);
        hvofSystem.Navigation(s => s.Subsystems).UsePropertyAccessMode(PropertyAccessMode.Field);
        hvofSystem.Navigation(s => s.Controllers).UsePropertyAccessMode(PropertyAccessMode.Field);

        var subsystem = builder.Entity<HvofSubsystem>();
        subsystem.HasKey(s => s.Id);
        subsystem.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        subsystem.Property(s => s.HvofSystemId).IsRequired();
        subsystem.Property(s => s.SubsystemType).HasUpperSnakeCaseConversion(30).IsRequired();
        subsystem.Property(s => s.Name).HasMaxLength(80).IsRequired();
        subsystem.Property(s => s.Alias).HasMaxLength(30).IsRequired();
        subsystem.HasIndex(s => new { s.HvofSystemId, s.Alias }).IsUnique();
        subsystem.OwnsMany(s => s.Parameters, parameter =>
        {
            parameter.ToTable("hvof_subsystem_parameters");
            parameter.WithOwner().HasForeignKey("HvofSubsystemId");
            parameter.Property<int>("Id").ValueGeneratedOnAdd();
            parameter.HasKey("Id");
            parameter.Property(p => p.Parameter).HasMaxLength(40).IsRequired();
        });
        subsystem.Navigation(s => s.Parameters).UsePropertyAccessMode(PropertyAccessMode.Field);
        subsystem.HasMany(s => s.Parts).WithOne().HasForeignKey(p => p.HvofSubsystemId).OnDelete(DeleteBehavior.Cascade);
        subsystem.Navigation(s => s.Parts).UsePropertyAccessMode(PropertyAccessMode.Field);

        var part = builder.Entity<HvofPart>();
        part.HasKey(p => p.Id);
        part.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        part.Property(p => p.HvofSubsystemId).IsRequired();
        part.Property(p => p.PartType).HasUpperSnakeCaseConversion(30).IsRequired();
        part.Property(p => p.SerialNumber).HasMaxLength(60).IsRequired();
        part.Property(p => p.Manufacturer).HasMaxLength(80).IsRequired();
        part.HasIndex(p => new { p.HvofSubsystemId, p.SerialNumber }).IsUnique();

        var controller = builder.Entity<Controller>();
        controller.HasKey(c => c.Id);
        controller.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        controller.Property(c => c.HvofSystemId).IsRequired();
        controller.Property(c => c.ControllerNumber).IsRequired();
        controller.Property(c => c.DeviceType).HasUpperSnakeCaseConversion(20).IsRequired();
        controller.Property(c => c.SerialNumber).HasMaxLength(60).IsRequired();
        controller.Property(c => c.Manufacturer).HasMaxLength(80).IsRequired();
        controller.Property(c => c.Model).HasMaxLength(80).IsRequired();
        controller.Property(c => c.PartNumber).HasMaxLength(40).IsRequired();
        controller.Property(c => c.IpAddress).HasMaxLength(45).IsRequired();
        controller.Property(c => c.EndpointUrl).HasMaxLength(200);
        controller.HasIndex(c => new { c.HvofSystemId, c.ControllerNumber }).IsUnique();
        controller.OwnsMany(c => c.SupportedProtocols, protocol =>
        {
            protocol.ToTable("controller_supported_protocols");
            protocol.WithOwner().HasForeignKey("ControllerId");
            protocol.Property<int>("Id").ValueGeneratedOnAdd();
            protocol.HasKey("Id");
            protocol.Property(p => p.Protocol).HasUpperSnakeCaseConversion(20).IsRequired();
        });
        controller.Navigation(c => c.SupportedProtocols).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
