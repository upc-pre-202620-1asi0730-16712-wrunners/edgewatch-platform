# Changelog

All notable changes to the EdgeWatch Platform are documented in this file. The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project adheres to [Semantic Versioning](https://semver.org/).

## [1.0.1] - 2026-11-11
### Added
- `.gitattributes` normalizing line endings to LF.

## [1.0.0] - 2026-11-11
### Added
- Shared kernel: `AggregateRoot` with domain events dispatched through Cortex.Mediator, `Result<T>` and `Error`, RFC 9457 problem details localized in English and Spanish, global exception handler, auditing interceptor and command pipeline behaviors.
- IAM: organizations, users with roles, sign-up (organization + administrator), sign-in with JWT, role assignment and custom authorization middleware; roles seeded on startup.
- Equipment: HVOF systems with subsystems, parameters, parts and controllers; recipes with threshold bands, applicabilities and DRAFT → ACTIVE → RETIRED lifecycle; update endpoints.
- Traceability: customers, recovered components and recuperation orders linked to spray sessions.
- Process Monitoring: spray sessions with frozen recipe bands, reading ingestion classified by band, deviations summary, complete and abort.
- Fault Diagnosis: cause-effect rules seeded per organization and fault cases opened from out-of-band readings, with acknowledge and resolve.
- Reporting: customizable report templates (branding, page setup, sections of widgets) seeded per organization and the registry of generated reports.
- OpenAPI documentation with Scalar, Dockerfile and README.

### Fixed
- Recuperation `DateOnly` columns are converted explicitly for the MySQL provider.
