# DTOs y mapeos

## DTOs por rol

### LoginRequest / LoginResponse

Usados en `POST /api/auth/login`.

```
LoginRequest { Email, Password }
    -> LoginResponse { AccessToken, ExpiresIn, User: UserDto }
        -> UserDto { Id, Nombre, Apellidos, Email, Role }
```

### MisFaltaDto (Alumno)

Usado en `GET /api/faltas/mis-faltas`. Muestra los datos basicos de la falta sin informacion de otros usuarios.

| Propiedad       | Tipo     |
|-----------------|----------|
| Id              | int      |
| IdUsuario       | int      |
| IdCurso         | int      |
| FechaIncidencia | DateTime |
| TipoFalta       | string   |
| EsJustificada   | bool     |
| Comentario      | string?  |

### AlumnoCursoDto (Alumno)

Usado en `GET /api/faltas/mis-cursos`. Lista los cursos en los que esta matriculado el alumno.

| Propiedad     | Tipo   |
|---------------|--------|
| IdCurso       | int    |
| Nombre        | string |
| DuracionHoras | int    |

### ResumenAsistenciaDto (Alumno)

Usado en `GET /api/faltas/resumen-asistencia`. Muestra el porcentaje de asistencia por curso.

| Propiedad            | Tipo    |
|----------------------|---------|
| IdCurso              | int     |
| NombreCurso          | string  |
| TotalHoras           | int     |
| HorasFalta           | int     |
| PorcentajeAsistencia | decimal |

### ProfesorCursoDto (Profesor)

Usado en `GET /api/profesor/cursos`. Lista los cursos asignados al profesor.

| Propiedad     | Tipo   |
|---------------|--------|
| IdCurso       | int    |
| Nombre        | string |
| DuracionHoras | int    |

### ProfesorAlumnoDto (Profesor)

Usado en `GET /api/profesor/cursos/{idCurso}/alumnos`. Lista los alumnos de un curso del profesor.

| Propiedad | Tipo   |
|-----------|--------|
| IdUsuario | int    |
| Nombre    | string |
| Apellidos | string |
| Email     | string |

### ProfesorFaltaDto (Profesor)

Usado en `GET /api/profesor/cursos/{idCurso}/faltas`. Extiende `MisFaltaDto` con informacion del alumno.

| Propiedad       | Tipo     |
|-----------------|----------|
| Id              | int      |
| IdUsuario       | int      |
| IdCurso         | int      |
| FechaIncidencia | DateTime |
| TipoFalta       | string   |
| EsJustificada   | bool     |
| Comentario      | string?  |
| NombreAlumno    | string   |
| ApellidosAlumno | string   |

### ProfesorFaltaRequest (Profesor)

Usado en `POST /api/profesor/faltas` para crear una falta.

| Propiedad       | Tipo     | Validacion        |
|-----------------|----------|-------------------|
| IdUsuario       | int      | Required          |
| IdCurso         | int      | Required          |
| FechaIncidencia | DateTime | Required          |
| TipoFalta       | string   | Required          |
| Comentario      | string?  | MaxLength(500)    |

### AdminCursoDto (Administrador)

Usado en `GET /api/admin/cursos`. Lista todos los cursos con estado activo.

| Propiedad     | Tipo    |
|---------------|---------|
| IdCurso       | int     |
| Nombre        | string  |
| DuracionHoras | int     |
| Activo        | bool    |

### AdminAlumnoDto (Administrador)

Usado en `GET /api/admin/cursos/{idCurso}/alumnos`. Lista alumnos de un curso.

| Propiedad     | Tipo   |
|---------------|--------|
| IdUsuario     | int    |
| Nombre        | string |
| Apellidos     | string |
| Email         | string |
| EstadoUsuario | string |

### AdminFaltaDto (Administrador)

Usado en `GET /api/admin/faltas` y `GET /api/admin/cursos/{idCurso}/faltas`. Incluye nombres resueltos de alumno y curso.

| Propiedad       | Tipo     |
|-----------------|----------|
| Id              | int      |
| IdAlumno        | int      |
| NombreAlumno    | string   |
| IdCurso         | int      |
| NombreCurso     | string   |
| Fecha           | DateTime |
| Tipo            | string   |
| EsJustificada   | bool     |
| Observaciones   | string?  |

### AdminFaltaJustificacionRequest

Usado en `PATCH /api/admin/faltas/{id}/justificacion`.

| Propiedad     | Tipo |
|---------------|------|
| EsJustificada | bool |

### UsuarioDto (Profesor, Administrador)

Usado en `GET /api/usuarios/{id}`. Informacion publica de un usuario.

| Propiedad      | Tipo   |
|----------------|--------|
| Id             | int    |
| Nombre         | string |
| Apellidos      | string |
| Email          | string |
| Role           | string |
| EstadoUsuario  | bool   |

## Metodos de mapeo

Definidos como extension methods en `Application/Common/`:

| Archivo                    | Metodo               | Conversion             |
|----------------------------|----------------------|------------------------|
| `FaltaMappings.cs`         | `ToMisFaltaDto()`    | `Falta -> MisFaltaDto` |
| `ProfesorFaltaMappings.cs` | `ToProfesorFaltaDto()`| `Falta -> ProfesorFaltaDto` |

El mapeo de `AdminFaltaDto` se realiza inline en `AdminFaltasService.GetAllFaltasAsync()`.

## Documentos relacionados

- [Endpoints](05-endpoints.md)
- [Modelo de datos](04-modelo-de-datos.md)
