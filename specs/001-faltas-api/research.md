# Research Notes: API de Gestión de Faltas Tajamar

## 1. Runtime and Framework Choice

- Decision: Use ASP.NET Core Web API on .NET 8 LTS.
- Rationale: It gives long-term support, mature JWT support, first-class HTTP client infrastructure, and a well-supported EF Core stack for SQL Server.
- Alternatives considered: .NET 6/7 (shorter support window), minimal-only API style without layering, or a custom host with lower-level primitives.

## 2. Architecture Style

- Decision: Organize the backend into Api, Application, Domain, and Infrastructure layers.
- Rationale: The feature has clear separations between HTTP transport, business rules, persistence, and external Tajamar integration.
- Alternatives considered: Single-project controllers with direct DbContext access, or splitting by feature without explicit domain boundaries.

## 3. Authentication Strategy

- Decision: Use a local JWT issued by the Tajamar backend for user-facing endpoints, while keeping the external Tajamar JWT only for server-side synchronization.
- Rationale: The user requirement explicitly says local endpoints must be protected and that alumno/profesor logins should be validated against mirror tables before issuing a local token.
- Alternatives considered: Forwarding the external JWT directly to the frontend, or using sessions instead of JWT.

## 4. External API Integration

- Decision: Implement a dedicated `TajamarApiClientService` backed by `HttpClientFactory` with admin login, bearer token attachment, token caching, and refresh-on-expiry behavior.
- Rationale: Centralizing the integration makes the admin synchronization flow reusable and avoids spreading token handling across controllers or services.
- Alternatives considered: Raw `HttpClient` instances in each caller, or logging in on every request without caching the token.

## 5. Persistence Strategy

- Decision: Use Entity Framework Core against SQL Server and map the provided tables as authoritative local schema.
- Rationale: The SQL script already defines the truth for entities, primary keys, relationships, and constraints, so the ORM should conform to it rather than redefine it.
- Alternatives considered: Dapper with hand-written SQL, database-first only, or a code-first schema that diverges from the supplied script.

## 6. Synchronization Model

- Decision: Refresh mirror tables through a server-side synchronization workflow that can run on startup and on a scheduled background interval, with a manual trigger available later if needed.
- Rationale: Mirror data must stay current for authentication and access control, and the system needs a server-owned way to keep it updated.
- Alternatives considered: Manual administration only, or relying on client requests to refresh mirror data opportunistically.

## 7. Authorization Model

- Decision: Enforce role-based authorization using local claims mapped to the three supported roles: alumno, profesor, and administrador.
- Rationale: The SQL schema and feature requirements already define these roles, so the API should align claims and policies to them directly.
- Alternatives considered: Free-form claim strings, ad hoc checks in controllers, or a generic permission matrix with no direct link to the mirror data.
