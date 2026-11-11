using System.Globalization;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using EdgeWatchPlatform.API.Equipment.Application.Acl;
using EdgeWatchPlatform.API.Equipment.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.Equipment.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Equipment.Interfaces.Acl;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;
using EdgeWatchPlatform.API.FaultDiagnosis.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Iam.Application.Acl;
using EdgeWatchPlatform.API.Iam.Application.CommandServices;
using EdgeWatchPlatform.API.Iam.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;
using EdgeWatchPlatform.API.Iam.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Iam.Infrastructure.Hashing.BCrypt.Services;
using EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EdgeWatchPlatform.API.Iam.Infrastructure.Tokens.Jwt.Configuration;
using EdgeWatchPlatform.API.Iam.Infrastructure.Tokens.Jwt.Services;
using EdgeWatchPlatform.API.Iam.Interfaces.Acl;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.CommandServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.OutboundServices.Acl;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.QueryServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;
using EdgeWatchPlatform.API.ProcessMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Application.Acl;
using EdgeWatchPlatform.API.Traceability.Interfaces.Acl;
using EdgeWatchPlatform.API.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;
using EdgeWatchPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Pipeline.Middleware.Components;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
using EdgeWatchPlatform.API.Shared.Resources;
using EdgeWatchPlatform.API.Shared.Resources.Errors;
using EdgeWatchPlatform.API.Traceability.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.Traceability.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using ProblemDetailsFactory = EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

// Controllers, kebab-case routes and validation ProblemDetails
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            var problem = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = localizer["ValidationErrorTitle"],
                Instance = context.HttpContext.Request.Path
            };
            return new BadRequestObjectResult(problem) { ContentTypes = { "application/problem+json" } };
        };
    });

// ProblemDetails for unexpected errors (RFC 9457)
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Localization (en default, es)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("es") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<ProblemDetailsFactory>();

// CORS for the web application
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
    .Select(Environment.ExpandEnvironmentVariables).ToArray() ?? [];
builder.Services.AddCors(options => options.AddPolicy("EdgeWatchWebApp", policy =>
    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

// OpenAPI + Scalar
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "EdgeWatch Platform API";
        document.Info.Version = "v1";
        document.Info.Description = "REST API for monitoring and tracing HVOF thermal-spray recuperation processes.";
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// Database
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var template = builder.Configuration.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("Database connection string is not set in the configuration.");
    var connectionString = Environment.ExpandEnvironmentVariables(template);
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
        .EnableDetailedErrors();
    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});

// Dependency injection — Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

// Dependency injection — IAM
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IRoleCommandService, RoleCommandService>();
builder.Services.AddScoped<IRoleQueryService, RoleQueryService>();
builder.Services.AddScoped<IOrganizationQueryService, OrganizationQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

// Dependency injection — Equipment
builder.Services.AddScoped<IHvofSystemRepository, HvofSystemRepository>();
builder.Services.AddScoped<IHvofSystemCommandService, HvofSystemCommandService>();
builder.Services.AddScoped<IHvofSystemQueryService, HvofSystemQueryService>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IRecipeCommandService, RecipeCommandService>();
builder.Services.AddScoped<IRecipeQueryService, RecipeQueryService>();
builder.Services.AddScoped<IEquipmentContextFacade, EquipmentContextFacade>();

// Dependency injection — Traceability
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerCommandService, CustomerCommandService>();
builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();
builder.Services.AddScoped<IRecoveredComponentRepository, RecoveredComponentRepository>();
builder.Services.AddScoped<IRecoveredComponentCommandService, RecoveredComponentCommandService>();
builder.Services.AddScoped<IRecoveredComponentQueryService, RecoveredComponentQueryService>();
builder.Services.AddScoped<IRecuperationRepository, RecuperationRepository>();
builder.Services.AddScoped<IRecuperationCommandService, RecuperationCommandService>();
builder.Services.AddScoped<IRecuperationQueryService, RecuperationQueryService>();
builder.Services.AddScoped<ITraceabilityContextFacade, TraceabilityContextFacade>();

// Dependency injection — Process Monitoring
builder.Services.AddScoped<ISpraySessionRepository, SpraySessionRepository>();
builder.Services.AddScoped<IProcessReadingRepository, ProcessReadingRepository>();
builder.Services.AddScoped<ExternalEquipmentService>();
builder.Services.AddScoped<ExternalTraceabilityService>();
builder.Services.AddScoped<ISpraySessionCommandService, SpraySessionCommandService>();
builder.Services.AddScoped<ISpraySessionQueryService, SpraySessionQueryService>();

// Dependency injection — Fault Diagnosis
builder.Services.AddScoped<IFaultRuleRepository, FaultRuleRepository>();
builder.Services.AddScoped<IFaultCaseRepository, FaultCaseRepository>();
builder.Services.AddScoped<IFaultRuleCommandService, FaultRuleCommandService>();
builder.Services.AddScoped<IFaultRuleQueryService, FaultRuleQueryService>();
builder.Services.AddScoped<IFaultCaseCommandService, FaultCaseCommandService>();
builder.Services.AddScoped<IFaultCaseQueryService, FaultCaseQueryService>();

// Mediator: command pipeline behaviors and domain event handlers discovered in this assembly
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// Apply pending migrations on startup (ADR-008) and seed the roles
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
    await scope.ServiceProvider.GetRequiredService<IRoleCommandService>().Handle(new SeedRolesCommand());
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRequestLocalization();
app.UseCors("EdgeWatchWebApp");
app.UseRequestAuthorization();
app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("EdgeWatch Platform API").WithTheme(ScalarTheme.Mars));
app.MapGet("/health", () => Results.Ok(new { status = "UP" })).ExcludeFromDescription();
app.MapControllers();
app.Run();
