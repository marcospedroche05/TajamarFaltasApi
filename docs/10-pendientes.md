# Tareas pendientes

Lista de mejoras y funcionalidades identificadas que faltan por implementar.

## Prioritarias

- [x] **Validacion de password en login**: Campo `Password` en `UsuarioMirror` implementado. Validacion directa en `AuthService` durante el flujo de login.
- [x] **Entidad UsuarioCurso**: Mapeada en Domain, nav properties en UsuarioMirror y CursoMirror, DbContext configurado con PK compuesta y FKs con DeleteBehavior.Restrict.
- [x] **Validacion profesor-curso**: `ProfesorFaltasService.CanProfesorAccessCursoAsync` ahora valida contra `UsuariosCursos`. Verificacion simultanea: relacion usuario-curso, usuario es Profesor activo, curso activo.

### Bloque Profesor — endpoints nuevos
- [x] `GET /api/profesor/cursos` — lista de cursos asignados al profesor autenticado.
- [x] `GET /api/profesor/cursos/{idCurso}/alumnos` — alumnos matriculados en un curso del profesor.
- [x] `DELETE /api/profesor/faltas/{id}` — eliminar cualquier falta de sus cursos asignados.

### Bloque Alumno — endpoints nuevos
- [x] `GET /api/faltas/mis-cursos` — cursos en los que esta matriculado el alumno autenticado.
- [x] `GET /api/faltas/resumen-asistencia` — porcentaje de asistencia por curso del alumno autenticado.

### Bloque Admin — endpoints nuevos
- [x] `GET /api/admin/cursos` — todos los cursos activos e inactivos.
- [x] `GET /api/admin/cursos/{idCurso}/alumnos` — alumnos matriculados en un curso especifico.
- [x] `GET /api/admin/cursos/{idCurso}/faltas` — todas las faltas de un curso especifico.
- [x] `DELETE /api/admin/faltas/{id}` — eliminar cualquier falta del sistema.

## Secundarias

- [ ] Filtros en endpoints de faltas: rango de fechas, tipo, estado de justificacion.
- [ ] Paginacion en `GET /api/admin/faltas`.
- [ ] `PUT /api/profesor/faltas/{id}` — editar tipo y comentario de una falta del curso.
- [ ] Eliminar dependencia de `TajamarApiClientService` si se confirma que no se usara.

## Documentos relacionados

- [Vision general](01-vision-general.md)
- [Modelo de datos](04-modelo-de-datos.md)
