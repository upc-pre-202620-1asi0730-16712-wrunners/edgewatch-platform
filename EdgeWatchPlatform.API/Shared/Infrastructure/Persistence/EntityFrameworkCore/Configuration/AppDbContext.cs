using EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.FaultDiagnosis.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.ProcessMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Bounded context configurations are applied here, one call per context.
        builder.ApplyIamConfiguration();
        builder.ApplyEquipmentConfiguration();
        builder.ApplyRecipeConfiguration();
        builder.ApplyTraceabilityConfiguration();
        builder.ApplyProcessMonitoringConfiguration();
        builder.ApplyFaultDiagnosisConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}
