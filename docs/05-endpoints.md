# Endpoints de la API

Base URL: `https://localhost:{puerto}/api`

## Autenticacion

### POST /api/auth/login

Autentica un usuario y devuelve un JWT.

**Request body:**
```json
{
  "email": "alumno@tajamar365.com",
  "password": "12345"
}
```

**Response 200:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 7200,
  "user": {
    "id": 1,
    "nombre": "Juan",
    "apellidos": "Garcia Lopez",
    "email": "alumno@tajamar365.com",
    "role": "Alumno"
  }
}
```

**Response 401:** Credenciales invalidas o usuario inactivo.

---

## Alumno (requiere JWT con role Alumno)

### GET /api/faltas/mis-faltas

Devuelve todas las faltas del alumno autenticado, ordenadas por fecha descendente.

**Response 200:**
```json
[
  {
    "id": 1,
    "idUsuario": 5,
    "idCurso": 2,
    "fechaIncidencia": "2026-05-15T09:00:00",
    "tipoFalta": "Retraso",
    "esJustificada": false,
    "comentario": "Llego 10 minutos tarde"
  }
]
```

### GET /api/faltas/mis-cursos

Devuelve los cursos activos en los que esta matriculado el alumno autenticado.

**Response 200:**
```json
[
  {
    "idCurso": 2,
    "nombre": "DAW 2o",
    "duracionHoras": 2000
  }
]
```

**Response estructura de `AlumnoCursoDto`:**

| Propiedad     | Tipo |
|---------------|------|
| IdCurso       | int  |
| Nombre        | string |
| DuracionHoras | int  |

### GET /api/faltas/resumen-asistencia

Devuelve el porcentaje de asistencia del alumno autenticado por curso. El calculo usa solo faltas de tipo "Falta" (excluye retrasos y salidas anticipadas) y la duracion total del curso.

**Response 200:**
```json
[
  {
    "idCurso": 2,
    "nombreCurso": "DAW 2o",
    "totalHoras": 2000,
    "horasFalta": 16,
    "porcentajeAsistencia": 99.2
  }
]
```

**Response estructura de `ResumenAsistenciaDto`:**

| Propiedad            | Tipo  |
|----------------------|-------|
| IdCurso              | int   |
| NombreCurso          | string |
| TotalHoras           | int   |
| HorasFalta           | int   |
| PorcentajeAsistencia | decimal |

---

## Profesor (requiere JWT con role Profesor)

### GET /api/profesor/cursos

Devuelve los cursos activos asignados al profesor autenticado.

**Response 200:**
```json
[
  {
    "idCurso": 2,
    "nombre": "DAW 2o",
    "duracionHoras": 2000
  }
]
```

**Response estructura de `ProfesorCursoDto`:**

| Propiedad     | Tipo |
|---------------|------|
| IdCurso       | int  |
| Nombre        | string |
| DuracionHoras | int  |

### GET /api/profesor/cursos/{idCurso}/alumnos

Devuelve los alumnos matriculados en un curso asignado al profesor autenticado.

**Parametros de ruta:**
- `idCurso` (int): ID del curso.

**Response 200:**
```json
[
  {
    "idUsuario": 5,
    "nombre": "Juan",
    "apellidos": "Garcia Lopez",
    "email": "alumno@tajamar365.com"
  }
]
```

**Response estructura de `ProfesorAlumnoDto`:**

| Propiedad | Tipo   |
|-----------|--------|
| IdUsuario | int    |
| Nombre    | string |
| Apellidos | string |
| Email     | string |

**Response 403:** El profesor no esta asignado a ese curso.
**Response 404:** Curso no encontrado.

### GET /api/profesor/cursos/{idCurso}/faltas

Devuelve todas las faltas de un curso asignado al profesor, enriquecidas con el nombre y apellidos del alumno.

**Parametros de ruta:**
- `idCurso` (int): ID del curso.

**Response 200:**
```json
[
  {
    "id": 1,
    "idUsuario": 5,
    "idCurso": 2,
    "fechaIncidencia": "2026-05-15T09:00:00",
    "tipoFalta": "Retraso",
    "esJustificada": false,
    "comentario": "Llego 10 minutos tarde",
    "nombreAlumno": "Juan",
    "apellidosAlumno": "Garcia Lopez"
  }
]
```

**Response 403:** El profesor no tiene acceso a ese curso.

### POST /api/profesor/faltas

Registra una nueva falta para un alumno.

**Request body:**
```json
{
  "idUsuario": 5,
  "idCurso": 2,
  "fechaIncidencia": "2026-05-20T08:30:00",
  "tipoFalta": "Falta",
  "comentario": "No se presento a clase"
}
```

**Valores validos para `tipoFalta`:** `"Falta"`, `"Retraso"`, `"Salida de antes"`, `"SalidaDeAntes"`

**Response 201:** Falta creada con `Location` header.
**Response 400:** Datos invalidos (alumno no existe, curso inactivo, tipo incorrecto).

### DELETE /api/profesor/faltas/{id}

Elimina una falta de un curso asignado al profesor autenticado.

**Parametros de ruta:**
- `id` (int): ID de la falta.

**Response 204:** Falta eliminada correctamente.
**Response 403:** El profesor no tiene acceso al curso de la falta.
**Response 404:** Falta no encontrada.

---

## Administrador (requiere JWT con role Administrador)

### GET /api/admin/cursos

Devuelve todos los cursos del sistema (activos e inactivos), ordenados por nombre.

**Response 200:**
```json
[
  {
    "idCurso": 2,
    "nombre": "DAW 2o",
    "duracionHoras": 2000,
    "activo": true
  }
]
```

**Response estructura de `AdminCursoDto`:**

| Propiedad     | Tipo    |
|---------------|---------|
| IdCurso       | int     |
| Nombre        | string  |
| DuracionHoras | int     |
| Activo        | bool    |

### GET /api/admin/cursos/{idCurso}/alumnos

Devuelve los alumnos matriculados en un curso especifico (solo alumnos, no profesores ni administradores).

**Parametros de ruta:**
- `idCurso` (int): ID del curso.

**Response 200:**
```json
[
  {
    "idUsuario": 5,
    "nombre": "Juan",
    "apellidos": "Garcia Lopez",
    "email": "alumno@tajamar365.com",
    "estadoUsuario": "Activo"
  }
]
```

**Response estructura de `AdminAlumnoDto`:**

| Propiedad      | Tipo   |
|----------------|--------|
| IdUsuario      | int    |
| Nombre         | string |
| Apellidos      | string |
| Email          | string |
| EstadoUsuario  | string |

**Response 404:** Curso no encontrado.

### GET /api/admin/cursos/{idCurso}/faltas

Devuelve todas las faltas registradas en un curso especifico, enriquecidas con el nombre del alumno.

**Parametros de ruta:**
- `idCurso` (int): ID del curso.

**Response 200:**
```json
[
  {
    "id": 1,
    "idAlumno": 5,
    "nombreAlumno": "Juan Garcia",
    "idCurso": 2,
    "nombreCurso": "DAW 2o",
    "fecha": "2026-05-15T09:00:00",
    "tipo": "Retraso",
    "esJustificada": false,
    "observaciones": "Llego tarde"
  }
]
```

**Response 404:** Curso no encontrado.

### GET /api/admin/faltas

Devuelve todas las faltas del sistema con informacion enriquecida (nombre alumno, nombre curso).

**Response 200:**
```json
[
  {
    "id": 1,
    "idAlumno": 5,
    "nombreAlumno": "Juan Garcia",
    "idCurso": 2,
    "nombreCurso": "DAW 2o",
    "fecha": "2026-05-15T09:00:00",
    "tipo": "Retraso",
    "esJustificada": false,
    "observaciones": "Llego tarde"
  }
]
```

### PATCH /api/admin/faltas/{id}/justificacion

Actualiza el estado de justificacion de una falta.

**Request body:**
```json
{
  "esJustificada": true
}
```

**Response 204:** Actualizado correctamente.
**Response 404:** Falta no encontrada.

### DELETE /api/admin/faltas/{id}

Elimina cualquier falta del sistema.

**Parametros de ruta:**
- `id` (int): ID de la falta.

**Response 204:** Falta eliminada correctamente.
**Response 404:** Falta no encontrada.

---

## Usuarios compartidos (requiere JWT con role Profesor o Administrador)

### GET /api/usuarios/{id}

Obtiene la informacion publica de un usuario. Accesible para profesores y administradores.

**Parametros de ruta:**
- `id` (int): ID del usuario.

**Response 200:**
```json
{
  "id": 5,
  "nombre": "Juan",
  "apellidos": "Garcia Lopez",
  "email": "juan@tajamar365.com",
  "role": "Alumno",
  "estadoUsuario": true
}
```

**Response 404:** Usuario no encontrado.

**Nota:** El campo `password` nunca es incluido en la respuesta.

## Documentos relacionados

- [Roles y permisos](03-roles-y-permisos.md)
- [Autenticacion](07-autenticacion.md)
- [DTOs y mapeos](08-dtos-y-mapeos.md)
