using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class IamModelConfiguration
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        var role = builder.Entity<Role>();
        role.HasKey(r => r.Id);
        role.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        role.Property(r => r.Name).HasUpperSnakeCaseConversion(40).IsRequired();
        role.HasIndex(r => r.Name).IsUnique();
        role.Ignore(r => r.StringName);

        var organization = builder.Entity<Organization>();
        organization.HasKey(o => o.Id);
        organization.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        organization.Property(o => o.Name).HasMaxLength(150).IsRequired();
        organization.Property(o => o.Ruc).HasConversion(r => r.Value, v => new Ruc(v)).HasMaxLength(11).IsRequired();
        organization.Property(o => o.OrganizationType).HasUpperSnakeCaseConversion(30).IsRequired();
        organization.Property(o => o.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        organization.Property(o => o.TimeZone).HasMaxLength(60).IsRequired();
        organization.HasIndex(o => o.Ruc).IsUnique();
        organization.Ignore(o => o.IsSupplier);

        var user = builder.Entity<User>();
        user.HasKey(u => u.Id);
        user.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        user.Property(u => u.FullName).HasMaxLength(120).IsRequired();
        user.Property(u => u.Email).HasConversion(e => e.Value, v => new EmailAddress(v)).HasMaxLength(120).IsRequired();
        user.Property(u => u.PasswordHash).HasMaxLength(100).IsRequired();
        user.Property(u => u.OrganizationId).IsRequired();
        user.Property(u => u.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        user.HasIndex(u => u.Email).IsUnique();
        user.HasOne<Organization>().WithMany().HasForeignKey(u => u.OrganizationId).OnDelete(DeleteBehavior.Restrict);
        user.OwnsMany(u => u.Roles, userRole =>
        {
            userRole.ToTable("user_roles");
            userRole.WithOwner().HasForeignKey("UserId");
            userRole.Property<int>("Id").ValueGeneratedOnAdd();
            userRole.HasKey("Id");
            userRole.Property(r => r.RoleId).IsRequired();
            userRole.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId).OnDelete(DeleteBehavior.Restrict);
            userRole.HasIndex("UserId", nameof(UserRole.RoleId)).IsUnique();
        });
        user.Navigation(u => u.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
