# Arquitectura

El proyecto sigue el patron **Clean Architecture** organizado en cuatro capas con dependencias unidireccionales hacia el centro (Domain).

```
TajamarFaltas.Api            --> Capa de presentacion (Controllers, Auth policies, Program.cs)
TajamarFaltas.Application    --> Capa de aplicacion (Interfaces de servicios, DTOs, Mappings)
TajamarFaltas.Infrastructure --> Capa de infraestructura (EF Core, Servicios concretos, Persistencia)
TajamarFaltas.Domain         --> Capa de dominio (Entidades, Enums, Interfaces de contratos externos)
```

## Estructura de carpetas

```
backend/
├── src/
│   ├── TajamarFaltas.Api/
│   │   ├── Controllers/          # AuthController, FaltasController, ProfesorFaltasController, AdminFaltasController
│   │   ├── Authorization/        # Policies por rol (AlumnoOnly, ProfesorOnly, AdministradorOnly)
│   │   └── Program.cs            # Configuracion de servicios, JWT, CORS, Swagger/Scalar
│   ├── TajamarFaltas.Application/
│   │   ├── Auth/                 # IAuthService, ITokenService, JwtOptions, modelos de login
│   │   ├── FaltaManagement/      # Interfaces de servicio y DTOs por rol
│   │   └── Common/               # Extension methods de mapeo (Falta -> DTO)
│   ├── TajamarFaltas.Infrastructure/
│   │   ├── Auth/                 # AuthService, TokenService (implementaciones)
│   │   ├── FaltaManagement/      # MisFaltasQueryService, ProfesorFaltasService, AdminFaltasService
│   │   ├── Persistence/          # TajamarDbContext (EF Core)
│   │   ├── ExternalServices/     # TajamarApiClientService (no utilizado actualmente)
│   │   └── DependencyInjection/  # Registro de servicios
│   └── TajamarFaltas.Domain/
│       ├── Entities/             # Falta, UsuarioMirror, CursoMirror, RoleMirror
│       ├── Enums/                # TipoFalta, RoleType
│       └── Interfaces/           # ITajamarApiClientService
└── tests/
    ├── TajamarFaltas.Api.IntegrationTests/
    ├── TajamarFaltas.Application.Tests/
    └── TajamarFaltas.Infrastructure.Tests/
```

## Stack tecnologico

| Componente       | Tecnologia                  |
|------------------|-----------------------------|
| Runtime          | .NET 8                      |
| Lenguaje         | C#                          |
| ORM              | Entity Framework Core       |
| Base de datos    | SQL Server (local)          |
| Autenticacion    | JWT Bearer                  |
| Documentacion API| Swagger + Scalar            |
| CORS             | Configurado para Angular dev (`localhost:4200`) |

## Documentos relacionados

- [Vision general](01-vision-general.md)
- [Modelo de datos](04-modelo-de-datos.md)
- [Inyeccion de dependencias](06-inyeccion-dependencias.md)
