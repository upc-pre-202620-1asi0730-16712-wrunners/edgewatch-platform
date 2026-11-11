using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class RecipeModelConfiguration
{
    public static void ApplyRecipeConfiguration(this ModelBuilder builder)
    {
        var recipe = builder.Entity<Recipe>();
        recipe.HasKey(r => r.Id);
        recipe.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        recipe.Property(r => r.HvofSystemId).IsRequired();
        recipe.Property(r => r.RecipeNumber).IsRequired();
        recipe.Property(r => r.Name).HasMaxLength(120).IsRequired();
        recipe.Property(r => r.PowderSpecification).HasMaxLength(200).IsRequired();
        recipe.Property(r => r.Status).HasUpperSnakeCaseConversion(20).IsRequired();
        recipe.HasIndex(r => new { r.HvofSystemId, r.RecipeNumber, r.Status });
        recipe.HasOne<HvofSystem>().WithMany().HasForeignKey(r => r.HvofSystemId).OnDelete(DeleteBehavior.Restrict);
        recipe.Ignore(r => r.IsActive);
        recipe.OwnsMany(r => r.Applicabilities, applicability =>
        {
            applicability.ToTable("recipe_applicabilities");
            applicability.WithOwner().HasForeignKey("RecipeId");
            applicability.Property<int>("Id").ValueGeneratedOnAdd();
            applicability.HasKey("Id");
            applicability.Property(a => a.ComponentType).HasMaxLength(30).IsRequired();
            applicability.Property(a => a.MachineModel).HasMaxLength(80).IsRequired();
            applicability.Property(a => a.Position).HasMaxLength(80).IsRequired();
        });
        recipe.Navigation(r => r.Applicabilities).UsePropertyAccessMode(PropertyAccessMode.Field);
        recipe.HasMany(r => r.Parameters).WithOne().HasForeignKey(p => p.RecipeId).OnDelete(DeleteBehavior.Cascade);
        recipe.Navigation(r => r.Parameters).UsePropertyAccessMode(PropertyAccessMode.Field);

        var parameter = builder.Entity<RecipeParameter>();
        parameter.HasKey(p => p.Id);
        parameter.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        parameter.Property(p => p.RecipeId).IsRequired();
        parameter.Property(p => p.Parameter).HasMaxLength(40).IsRequired();
        foreach (var column in new[] { "Setpoint", "LowerShutdown", "LowerWarning", "NominalMin", "NominalMax", "UpperWarning", "UpperShutdown" })
            parameter.Property<decimal>(column).HasPrecision(12, 4).IsRequired();
        parameter.Property(p => p.UnitSymbol).HasMaxLength(15).IsRequired();
        parameter.Property(p => p.UnitCategory).HasUpperSnakeCaseConversion(20).IsRequired();
        parameter.HasIndex(p => new { p.RecipeId, p.Parameter }).IsUnique();
        parameter.Ignore(p => p.Bands);
    }
}
