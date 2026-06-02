# HTTP API Contract: API de Gestión de Faltas Tajamar

## Authentication

### POST /api/auth/login

Authenticates a local user against the mirror tables and returns a local JWT.

Request body:

```json
{
  "email": "usuario@tajamar.es",
  "password": "local-or-external-credential"
}
```

Response 200:

```json
{
  "accessToken": "eyJ...",
  "expiresIn": 3600,
  "user": {
    "id": 123,
    "nombre": "Nombre",
    "apellidos": "Apellidos",
    "email": "usuario@tajamar.es",
    "role": "Alumno"
  }
}
```

## Alumno Endpoints

### GET /api/faltas/mis-faltas

Returns the authenticated alumno's own absences only.

Authorization: `Bearer <local-jwt>`

Response 200:

```json
[
  {
    "id": 1,
    "idUsuario": 123,
    "idCurso": 45,
    "fechaIncidencia": "2026-05-26T08:30:00",
    "tipoFalta": "Retraso",
    "esJustificada": false,
    "comentario": "Llegó tarde por transporte"
  }
]
```

## Profesor Endpoints

### GET /api/profesor/cursos/{idCurso}/faltas

Returns absences for a course only when the authenticated profesor is authorized for that course.

### POST /api/profesor/faltas

Creates a new absence within the profesor's authorized scope.

Request body:

```json
{
  "idUsuario": 123,
  "idCurso": 45,
  "fechaIncidencia": "2026-05-26T08:30:00",
  "tipoFalta": "Falta",
  "comentario": "Sin justificación"
}
```

## Administrador Endpoints

### GET /api/admin/faltas

Returns all absences in the system.

### PATCH /api/admin/faltas/{id}/justificacion

Updates the justification state for an existing absence.

Request body:

```json
{
  "esJustificada": true
}
```

## Common Error Contract

```json
{
  "error": "string",
  "details": "string"
}
```
