# Roles y permisos

## Roles del sistema

El sistema define tres roles en el enum `RoleType` (`Domain/Enums/RoleType.cs`):

| Valor | Rol            | Descripcion                                      |
|-------|----------------|--------------------------------------------------|
| 1     | Profesor       | Registra y consulta faltas de sus cursos         |
| 2     | Alumno         | Consulta sus propias faltas                      |
| 3     | Administrador  | Consulta todas las faltas y gestiona justificaciones |

## Politicas de autorizacion

Definidas en `Api/Authorization/AuthorizationExtensions.cs` y referenciadas mediante `PolicyNames`:

| Policy                  | Claim requerido          | Controlador protegido       |
|-------------------------|---------------------------|-----------------------------|
| `AlumnoOnly`            | `Role = "Alumno"`        | `FaltasController`          |
| `ProfesorOnly`          | `Role = "Profesor"`      | `ProfesorFaltasController`  |
| `AdministradorOnly`     | `Role = "Administrador"` | `AdminFaltasController`     |
| `ProfesorOrAdministrador`| `Role = "Profesor" OR "Administrador"` | `UsuariosController` |

## Flujo de autorizacion

1. El usuario se autentica via `POST /api/auth/login` y recibe un JWT.
2. El JWT contiene los claims `NameIdentifier` (id), `Name`, `Email` y `Role`.
3. Cada controlador aplica `[Authorize(Policy = "...")]` a nivel de clase.
4. Los controllers extraen el `NameIdentifier` del token para filtrar datos por usuario.

## Documentos relacionados

- [Autenticacion](07-autenticacion.md)
- [Endpoints](05-endpoints.md)
