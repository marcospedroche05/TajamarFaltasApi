# Autenticacion

## Flujo de login

```
Cliente                         API
  │                              │
  ├── POST /api/auth/login ─────>│
  │   { email, password }        │
  │                              ├── Busca usuario por email en UsuariosMirror
  │                              ├── Verifica que EstadoUsuario == true
  │                              ├── Valida password
  │                              ├── Genera JWT con claims
  │<──── 200 { accessToken } ────┤
  │                              │
  ├── GET /api/faltas/mis-faltas ─>│  (Authorization: Bearer <token>)
  │                              ├── Valida JWT (firma, issuer, audience, expiry)
  │                              ├── Extrae claims y verifica policy
  │<──── 200 [...faltas] ────────┤
```

## Generacion del JWT

Clase: `Infrastructure/Auth/TokenService.cs`

El token incluye los siguientes claims:

| Claim              | Valor                              |
|--------------------|------------------------------------|
| `NameIdentifier`   | `usuario.Id` (int como string)     |
| `Name`             | `"{Nombre} {Apellidos}"`           |
| `Email`            | `usuario.Email`                    |
| `Role`             | `usuario.IdRole.ToString()` (ej: "Alumno") |

### Configuracion JWT (`appsettings.json` seccion `"Jwt"`)

| Propiedad           | Valor por defecto                             |
|---------------------|-----------------------------------------------|
| `Issuer`            | `"TajamarFaltasApi"`                          |
| `Audience`          | `"TajamarFaltasFrontend"`                     |
| `SigningKey`        | `"development-signing-key-development-signing-key"` |
| `AccessTokenMinutes`| `120` (2 horas)                               |

## Validacion del token (middleware)

Configurada en `Program.cs`:

- `ValidateIssuer`: true
- `ValidateAudience`: true
- `ValidateIssuerSigningKey`: true
- `ValidateLifetime`: true
- `ClockSkew`: 2 minutos

## Notas de seguridad

Esta API es exclusivamente para desarrollo/pruebas:
- Las contrasenas se almacenan en texto plano en la columna `Password` de `UsuariosMirror`. Durante el login, `AuthService` compara directamente el campo con la contrasena proporcionada.
- Todas las contrasenas de usuarios de prueba son `"12345"`.
- La signing key esta hardcodeada en `appsettings.json`.
- No se implementa refresh token.
- No hay rate limiting ni bloqueo por intentos fallidos.

## Documentos relacionados

- [Roles y permisos](03-roles-y-permisos.md)
- [Endpoints](05-endpoints.md)
