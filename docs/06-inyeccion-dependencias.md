# Inyeccion de dependencias

## Registro de servicios

La configuracion se reparte en dos extension methods invocados desde `Program.cs`:

### Application layer (`AddApplication`)

Archivo: `Application/DependencyInjection/ServiceCollectionExtensions.cs`

Actualmente vacio. Reservado para registrar servicios de la capa de aplicacion si se necesitan en el futuro.

### Infrastructure layer (`AddInfrastructure`)

Archivo: `Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`

| Interfaz                  | Implementacion            | Lifetime |
|---------------------------|---------------------------|----------|
| `IAuthService`            | `AuthService`             | Scoped   |
| `ITokenService`           | `TokenService`            | Scoped   |
| `IMisFaltasQueryService`  | `MisFaltasQueryService`   | Scoped   |
| `IProfesorFaltasService`  | `ProfesorFaltasService`   | Scoped   |
| `IAdminFaltasService`     | `AdminFaltasService`      | Scoped   |
| `IAdminCursosService`     | `AdminCursosService`      | Scoped   |
| `IUsuariosQueryService`   | `UsuariosQueryService`    | Scoped   |
| `ITajamarApiClientService`| `TajamarApiClientService` | HttpClient (transient handler) |
| `TajamarDbContext`        | EF Core SQL Server        | Scoped   |

### Configuracion en Program.cs

Ademas del registro de capas, `Program.cs` configura:

- **JWT Authentication**: lectura de `JwtOptions` desde `appsettings.json`, seccion `"Jwt"`.
- **Authorization policies**: via `AddTajamarAuthorizationPolicies()`.
- **CORS**: origenes permitidos `localhost:4200` y `127.0.0.1:4200`.
- **Swagger/Scalar**: documentacion API con esquema de seguridad Bearer.

## Documentos relacionados

- [Arquitectura](02-arquitectura.md)
- [Autenticacion](07-autenticacion.md)
