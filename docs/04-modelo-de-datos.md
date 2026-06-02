# Modelo de datos

## Diagrama de entidades

```
RoleMirror (1) ──── (*) UsuarioMirror (1) ──── (*) Falta (*) ──── (1) CursoMirror
                                      \                                    /
                                       └──── (*) UsuarioCurso (*) ────────┘
```

## Tablas

### RolesMirror

| Columna    | Tipo       | Restricciones          |
|------------|------------|------------------------|
| IdRole     | byte (PK)  | No auto-generado       |
| Rolename   | varchar(50)| Required, Unique index |

Valores: `1 = Profesor`, `2 = Alumno`, `3 = Administrador`

### UsuariosMirror

| Columna       | Tipo          | Restricciones              |
|---------------|---------------|----------------------------|
| Id            | int (PK)      | Auto-generado              |
| Nombre        | varchar(100)  | Required                   |
| Apellidos     | varchar(150)  | Required                   |
| Email         | varchar(254)  | Required, Unique index     |
| EstadoUsuario | bit           | Indica si esta activo      |
| Imagen        | varchar(500)  | Nullable                   |
| Password      | varchar(100)  | Required, texto plano (entorno test) |
| IdRole        | byte (FK)     | Referencia a RolesMirror   |

### CursosMirror

| Columna       | Tipo          | Restricciones       |
|---------------|---------------|---------------------|
| IdCurso       | int (PK)      | Auto-generado       |
| Nombre        | varchar(150)  | Required            |
| DuracionHoras | int           | Required            |
| Activo        | bit           | Indica si esta activo|

### UsuariosCursos (tabla de relacion)

Tabla de relacion N:M entre usuarios y cursos. Permite saber que profesores/alumnos estan asignados a que cursos.

| Columna   | Tipo      | Restricciones                                    |
|-----------|-----------|--------------------------------------------------|
| IdUsuario | int (PK)  | Parte de PK compuesta, FK a UsuariosMirror       |
| IdCurso   | int (PK)  | Parte de PK compuesta, FK a CursosMirror         |

PK compuesta: `(IdUsuario, IdCurso)`. Ambas FK tienen `ON DELETE RESTRICT`.

### Faltas

| Columna         | Tipo            | Restricciones                              |
|-----------------|-----------------|--------------------------------------------|
| Id              | int (PK)        | Auto-generado                              |
| IdUsuario       | int (FK)        | Referencia a UsuariosMirror, ON DELETE RESTRICT |
| IdCurso         | int (FK)        | Referencia a CursosMirror, ON DELETE RESTRICT  |
| FechaIncidencia | datetime2(0)    | Fecha y hora de la incidencia              |
| TipoFalta       | varchar(20)     | "Falta", "Retraso" o "Salida de antes"     |
| EsJustificada   | bit             | Default: false                             |
| Comentario      | varchar(500)    | Nullable                                   |

### Indices

- `Faltas`: indice compuesto en `(IdUsuario, FechaIncidencia)` para consultas de alumno.
- `Faltas`: indice compuesto en `(IdCurso, FechaIncidencia)` para consultas de profesor.

## Enum TipoFalta

```csharp
public enum TipoFalta
{
    Falta = 1,
    Retraso = 2,
    SalidaDeAntes = 3
}
```

Se almacena como string en la base de datos mediante un `ValueConverter`.

## Documentos relacionados

- [Arquitectura](02-arquitectura.md)
- [Endpoints](05-endpoints.md)
