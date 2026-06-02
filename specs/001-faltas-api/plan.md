# Implementation Plan: API de Gestión de Faltas Tajamar

**Branch**: `[001-faltas-api]` | **Date**: 2026-05-26 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-faltas-api/spec.md`

## Summary

Construir una Web API en .NET 8 para gestionar faltas con control de acceso por rol, autenticación local con JWT, sincronización de datos espejo desde la API externa de Tajamar y persistencia en SQL Server mediante Entity Framework Core. La API expondrá documentación interactiva con Scalar para explorar y probar endpoints protegidos con Bearer Token.

## Technical Context

**Language/Version**: C# / .NET 8 LTS

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core, SQL Server provider, `HttpClientFactory`, JWT bearer authentication, hosted services for synchronization, `Scalar.AspNetCore` for OpenAPI UI

**Storage**: SQL Server local con las tablas `RolesMirror`, `UsuariosMirror`, `CursosMirror` y `Faltas`

**Testing**: xUnit, integration tests with `WebApplicationFactory`, and focused service/unit tests for auth, authorization, and synchronization logic

**Target Platform**: Windows development and deployment target for a server-side Web API

**Project Type**: Web service / API backend

**Performance Goals**: Reads and writes for local endpoints should complete in under 500 ms at the application layer under normal load; external synchronization can run asynchronously

**Constraints**: External Tajamar credentials must come from environment variables or secret storage; local endpoints must never trust role data without validating the mirror tables; database constraints from the provided SQL script are authoritative; when the auth client calls the external Tajamar login endpoint, the JSON payload must map the locally received `Email` value to the external `userName` field and send the password as `password` without renaming those external property names

**API Documentation**: Scalar will be mapped from `Program.cs` over the generated OpenAPI document, preferably at `/scalar` so developers can authorize requests interactively with a JWT bearer token.

**Scale/Scope**: Single backend service with role-based access for alumno, profesor, and administrador flows plus periodic or on-demand mirror synchronization

## Constitution Check

No project-specific constitution has been ratified yet in this workspace, so the plan uses the feature requirements as the active quality gate set.

### Gate Review

- Security: pass. Secrets stay outside source control and the external admin token is confined to server-side synchronization.
- Testability: pass. Authentication, authorization, data access, and external integration are separable and testable in isolation.
- Simplicity: pass. The design keeps a single backend boundary and avoids introducing extra services before they are needed.
- Data integrity: pass. The SQL script defines the local schema and its constraints, and the implementation must conform to it exactly.
- Documentation UX: pass. Scalar provides the interactive OpenAPI experience without replacing the existing API boundaries or security model.

## Project Structure

### Documentation (this feature)

```text
specs/001-faltas-api/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── http-api.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── src/
│   ├── TajamarFaltas.Api/
│   │   ├── Controllers/
│   │   ├── Authorization/
│   │   ├── Middleware/
│   │   └── Program.cs  # Registers OpenAPI, Scalar, auth, and endpoint routing
│   ├── TajamarFaltas.Application/
│   │   ├── Auth/
│   │   ├── FaltaManagement/
│   │   ├── Synchronization/
│   │   └── Common/
│   ├── TajamarFaltas.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── Interfaces/
│   └── TajamarFaltas.Infrastructure/
│       ├── Persistence/
│       ├── ExternalServices/
│       ├── Authentication/
│       └── DependencyInjection/
└── tests/
    ├── TajamarFaltas.Api.IntegrationTests/
    ├── TajamarFaltas.Application.Tests/
    └── TajamarFaltas.Infrastructure.Tests/
```

**Structure Decision**: Use a layered backend-only solution under `backend/` because the repository currently contains no application source and the Angular frontend is external to this workspace. The split isolates HTTP concerns, business rules, persistence, and external integration while keeping the implementation easy to test.

## API Documentation Plan

- Install `Scalar.AspNetCore` in the API project alongside the existing ASP.NET Core OpenAPI setup.
- Generate the OpenAPI document from the API project and map Scalar in `Program.cs` at `/scalar` or an equivalent documented route.
- Configure Scalar to support authenticated requests by allowing developers to paste a local JWT Bearer token into the UI before calling protected endpoints.
- Keep documentation strictly server-side so the interactive explorer does not weaken the local authorization model.

## Complexity Tracking

No constitution violations require justification for this feature.
