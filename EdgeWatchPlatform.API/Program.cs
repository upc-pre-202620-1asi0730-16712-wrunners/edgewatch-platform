using System.Globalization;
using EdgeWatchPlatform.API.Equipment.Application.Internal.QueryServices;
using EdgeWatchPlatform.API.Equipment.Application.Internal.CommandServices;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
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
builder.Services.AddProblemDetails();

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
});

// Database
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var template = builder.Configuration.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("Database connection string is not set in the configuration.");
    var connectionString = Environment.ExpandEnvironmentVariables(template);
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();
    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});

// Dependency injection — Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Dependency injection — bounded contexts (one block per context)


// Dependency injection — Equipment
builder.Services.AddScoped<IHvofSystemRepository, HvofSystemRepository>();
builder.Services.AddScoped<IHvofSystemCommandService, HvofSystemCommandService>();
builder.Services.AddScoped<IHvofSystemQueryService, HvofSystemQueryService>();

// Dependency injection — Traceability
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerCommandService, CustomerCommandService>();
builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();
builder.Services.AddScoped<IRecoveredComponentRepository, RecoveredComponentRepository>();
builder.Services.AddScoped<IRecoveredComponentCommandService, RecoveredComponentCommandService>();
builder.Services.AddScoped<IRecoveredComponentQueryService, RecoveredComponentQueryService>();

var app = builder.Build();


// Apply pending migrations on startup (ADR-008)
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRequestLocalization();
app.UseCors("EdgeWatchWebApp");

app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("EdgeWatch Platform API").WithTheme(ScalarTheme.Mars));

app.MapGet("/health", () => Results.Ok(new { status = "UP" })).ExcludeFromDescription();
app.MapControllers();

app.Run();