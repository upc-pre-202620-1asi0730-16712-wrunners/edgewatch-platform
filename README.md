# EdgeWatch Platform

REST API of **EdgeWatch**, the platform by **WebRunners** for the traceability and process monitoring of HVOF thermal-spray component recuperation. Built with ASP.NET Core 10, Entity Framework Core 10 and MySQL, following Domain-Driven Design with one bounded context per business capability.

## Bounded contexts

| Context | Aggregates | Endpoints |
|---|---|---|
| **IAM** | `Organization`, `User`, `Role` | `POST /users` (sign-up), `POST /tokens` (sign-in), `GET /roles`, `GET /users?organizationId`, `GET /users/me`, `PUT /users/{id}/roles` |
| **Equipment** | `HvofSystem` (subsystems, parts, controllers), `Recipe` (parameters, applicabilities) | `/hvof-systems`, `/hvof-systems/{id}/subsystems`, `/hvof-systems/{id}/subsystems/{id}/parts`, `/hvof-systems/{id}/controllers`, `/recipes`, `PATCH /recipes/{id}/status` |
| **Traceability** | `Customer`, `RecoveredComponent`, `Recuperation` | `/customers`, `/components`, `/recuperations`, `PATCH /recuperations/{id}` |
| **Process Monitoring** | `SpraySession` (frozen recipe bands), `ProcessReading` | `/spray-sessions`, `PATCH /spray-sessions/{id}`, `/spray-sessions/{id}/readings`, `/spray-sessions/{id}/deviations` |
| **Fault Diagnosis** | `FaultRule`, `FaultCase` | `/fault-rules`, `/fault-cases`, `PATCH /fault-cases/{id}` |
| **Reporting** | `ReportTemplate` (sections, widgets), `GeneratedReport` | `/report-templates`, `PATCH /report-templates/{id}/status`, `/generated-reports` |

All routes live under `/api/v1` in kebab-case. Every endpoint except sign-up, sign-in and roles requires `Authorization: Bearer <token>`.

## Architecture

Each bounded context has the layers `Domain` (aggregates, entities, value objects, commands, queries, events, repository contracts), `Application` (command and query services, event handlers, outbound services and the anti-corruption-layer facade), `Infrastructure` (EF Core configuration and repositories, hashing, tokens, middleware) and `Interfaces` (REST controllers, resources, assemblers and the ACL contract).

- **Results and errors:** command services return `Result<T>` with a domain `Error` and the context error enum; `ProblemDetailsFactory` turns failures into RFC 9457 problem details localized in English and Spanish (`Accept-Language`).
- **Domain events:** aggregates extend `AggregateRoot` and record `IEvent`s; `DispatchDomainEventsInterceptor` publishes them through Cortex.Mediator when the unit of work is saved. Fault Diagnosis opens cases by handling Process Monitoring's `OutOfBandReadingDetectedEvent`.
- **Reporting:** templates describe branding, page setup and ordered sections of widgets (variable, aggregation, view); the web application renders the PDF from them and registers each generated report. Standard templates are seeded per organization on the first request.
- **Context integration:** Process Monitoring talks to Equipment and Traceability only through `IEquipmentContextFacade` and `ITraceabilityContextFacade`.
- **Persistence:** snake_case pluralized tables, audit timestamps by interceptor, migrations applied on startup.

## Requirements

- .NET SDK 10
- MySQL 8.x
- `dotnet-ef` (`dotnet tool install --global dotnet-ef`)

## Getting started

```bash
setx EDGEWATCH_DB_PASSWORD "<mysql root password>"   # Windows; open a new terminal afterwards
cd EdgeWatchPlatform.API
dotnet run
```

The API listens on `http://localhost:5262`; the Scalar documentation is at `http://localhost:5262/scalar`. The database `edgewatch-wa` and its tables are created by the migrations on the first run.

### Adding a migration

```bash
cd EdgeWatchPlatform.API
dotnet ef migrations add <Name> --output-dir Shared/Infrastructure/Persistence/EntityFrameworkCore/Migrations
```

## Configuration

| Setting | Where | Notes |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `appsettings.*.json` | `%VAR%` placeholders are expanded from environment variables |
| `TokenSettings:Secret` | `appsettings.*.json` | JWT signing key (`%EDGEWATCH_JWT_SECRET%` in production) |
| `Cors:AllowedOrigins` | `appsettings.*.json` | Origins of the web application |

## Docker

```bash
docker build -t edgewatch-platform .
```
