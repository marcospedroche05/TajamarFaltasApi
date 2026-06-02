# Configuracion

## appsettings.json

El archivo de configuracion principal esta en `Api/appsettings.json` y contiene las siguientes secciones:

### ConnectionStrings

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=ProyectoFaltas;..."
  }
}
```

- Base de datos: `ProyectoFaltas` en SQL Server local (`LOCALHOST\DEVELOPER`).
- Autenticacion: SQL Server Authentication con usuario `SA`.

### Jwt

```json
{
  "Jwt": {
    "Issuer": "TajamarFaltasApi",
    "Audience": "TajamarFaltasFrontend",
    "SigningKey": "development-signing-key-development-signing-key",
    "AccessTokenMinutes": 120
  }
}
```

Clase de opciones: `Application/Auth/JwtOptions.cs`

### TajamarApi

```json
{
  "TajamarApi": {
    "BaseUrl": "https://apicharlasalumnostajamartesting.azurewebsites.net/",
    "AdminUser": "admin@tajamar365.com",
    "AdminPassword": "12345"
  }
}
```

Clase de opciones: `Infrastructure/Options/TajamarApiOptions.cs`

> **Nota**: Esta seccion existe por compatibilidad pero no se utiliza activamente. La API trabaja exclusivamente con datos locales.

### CORS

Configurado en `Program.cs` para permitir peticiones desde el servidor de desarrollo Angular:

- `http://localhost:4200`
- `http://127.0.0.1:4200`

Permite cualquier header, cualquier metodo HTTP y credenciales.

## Documentacion de API interactiva

En modo Development:

- **Swagger JSON**: `/openapi/v1.json`
- **Scalar UI**: `/scalar`

Scalar incluye autenticacion Bearer persistente para facilitar pruebas.

## Documentos relacionados

- [Arquitectura](02-arquitectura.md)
- [Autenticacion](07-autenticacion.md)
