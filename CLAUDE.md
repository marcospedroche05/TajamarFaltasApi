# CLAUDE.md — TajamarFaltas API

## Descripcion del proyecto

API REST en .NET 8 / C# para gestionar faltas de asistencia del centro educativo Tajamar. Es un backend de **pruebas** para el frontend Angular. No conecta con APIs externas; usa exclusivamente datos locales en SQL Server.

Base de datos: `ProyectoFaltas` en `LOCALHOST\DEVELOPER` (SQL Server, usuario SA).

## Arquitectura

Clean Architecture con cuatro capas:

```
Api            → Controllers, Authorization policies, Program.cs
Application    → Interfaces de servicios, DTOs, Mappings
Infrastructure → EF Core (DbContext, servicios concretos)
Domain         → Entidades, Enums, Interfaces
```

Ruta base del codigo: `backend/src/`

## Convenciones de codigo

- Idioma del codigo: C# con nombres en **espanol** para entidades de negocio (Falta, Usuario, Curso) y en **ingles** para infraestructura y patrones (Controller, Service, Repository).
- Namespaces siguen la estructura de carpetas: `TajamarFaltas.{Capa}.{Subcarpeta}`.
- DTOs terminan en `Dto`, requests en `Request`, responses en `Response`.
- Mapeos como extension methods estaticos en `Application/Common/`.
- Servicios registrados como `Scoped` en `Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`.
- Sin migraciones de EF Core: la base de datos se gestiona con scripts SQL manuales en `sql/`.
- Entidades "Mirror" (UsuarioMirror, CursoMirror, RoleMirror) replican datos de la API externa de Tajamar para uso local. Sus PKs usan `ValueGeneratedNever()`.

## Documentacion

La carpeta `docs/` contiene documentacion atomica del proyecto. **Leer siempre antes de implementar.**

| Archivo | Contenido |
|---|---|
| `docs/01-vision-general.md` | Contexto, objetivo y alcance |
| `docs/02-arquitectura.md` | Capas, carpetas, stack |
| `docs/03-roles-y-permisos.md` | Roles, policies, autorizacion |
| `docs/04-modelo-de-datos.md` | Tablas, columnas, enums |
| `docs/05-endpoints.md` | API REST completa con ejemplos |
| `docs/06-inyeccion-dependencias.md` | Registro de servicios |
| `docs/07-autenticacion.md` | JWT, claims, flujo login |
| `docs/08-dtos-y-mapeos.md` | DTOs y extension methods |
| `docs/09-configuracion.md` | appsettings, CORS, Swagger |
| `docs/10-pendientes.md` | Tareas pendientes priorizadas |

---

# Sistema multi-agente

## Principios generales

1. **Tu (Claude Code) eres el Orquestador.** Planificas, delegas y validas. Usas modelo Opus.
2. **Antes de cualquier tarea, lee `docs/10-pendientes.md` y la documentacion relevante.** Si algo no esta claro, pregunta al usuario antes de implementar.
3. **Delega trabajo a agentes especializados** definidos en `.claude/agents/`. Cada agente tiene su propio archivo con rol, modelo y contexto completo.
4. **Al terminar cada tarea, lanza el agente de documentacion** para actualizar `docs/`.
5. **Nunca asumas.** Si la documentacion es ambigua o contradice el codigo, pregunta al usuario.

## Agentes disponibles

Los agentes estan definidos en `.claude/agents/`:

| Agente | Archivo | Modelo | Responsabilidad |
|---|---|---|---|
| **Orquestador** | `.claude/agents/orquestador.md` | Opus | Planifica, coordina, delega y supervisa. Agente principal. |
| **Implementador** | `.claude/agents/implementador.md` | Sonnet | Codigo C#: entidades, servicios, controllers, DTOs, mapeos |
| **Base de Datos** | `.claude/agents/base-de-datos.md` | Sonnet | Scripts T-SQL, coherencia DbContext vs esquema, datos de prueba |
| **Documentacion** | `.claude/agents/documentacion.md` | Haiku | Actualiza `docs/` tras cada tarea completada |
| **Revisor** | `.claude/agents/revisor.md` | Opus | Revisa coherencia, convenciones, seguridad antes de cerrar features |

### Como invocar agentes

Usa la herramienta `Agent` con el campo `model` correspondiente. El prompt debe incluir:
- La tarea concreta con criterios de aceptacion.
- Los archivos que necesita leer o modificar.
- El contexto minimo necesario (no duplicar lo que ya tiene en su archivo de agente).

Ejemplo:
```
Agent({
  description: "Implementar validacion password",
  model: "sonnet",
  prompt: "Lee .claude/agents/implementador.md para tu rol e instrucciones.\n\nTAREA: Anadir campo Password a UsuarioMirror y validar en AuthService...\n\nARCHIVOS: ..."
})
```

## Flujo de trabajo del orquestador

Cuando el usuario pida implementar algo, sigue este flujo:

```
1. ANALIZAR
   ├── Lee docs/10-pendientes.md
   ├── Lee la documentacion relevante (docs/)
   ├── Identifica archivos afectados con Glob/Grep
   └── Si hay ambiguedad → PREGUNTA al usuario

2. PLANIFICAR
   ├── Descompone la tarea en subtareas
   ├── Identifica que agentes necesita
   ├── Determina el orden (dependencias entre subtareas)
   └── Presenta el plan al usuario si la tarea es grande (>3 archivos)

3. EJECUTAR
   ├── Si hay cambios de BBDD → Agente Base de Datos primero
   ├── Implementacion → Agente Implementador (o tu mismo para cambios pequenos)
   ├── Subtareas independientes → lanza agentes en paralelo
   └── Cada agente recibe contexto completo (archivos, convenciones, tarea)

4. VALIDAR
   ├── Verifica que el codigo compila: dotnet build backend/src/TajamarFaltas.Api/
   ├── Si es feature compleja → Agente Revisor
   └── Resuelve hallazgos criticos antes de continuar

5. DOCUMENTAR
   └── Lanza Agente Documentacion con resumen de los cambios realizados

6. REPORTAR
   └── Resume al usuario: que se hizo, que archivos cambiaron, que queda pendiente
```

### Ejemplo de orquestacion

Si el usuario pide: "Implementa la validacion de password en el login"

1. **Lees** `docs/07-autenticacion.md` y `docs/10-pendientes.md`
2. **Planificas**: necesita columna Password en BBDD + campo en entidad + validacion en AuthService
3. **Agente Base de Datos** (sonnet): script SQL para ALTER TABLE + UPDATE datos
4. **Agente Implementador** (sonnet): modifica UsuarioMirror, DbContext, AuthService
5. **Compilas**: `dotnet build backend/src/TajamarFaltas.Api/`
6. **Agente Revisor** (opus): revisa cambios si es feature compleja
7. **Agente Documentacion** (haiku): actualiza docs/07, docs/04, docs/10
8. **Reportas** al usuario

## Reglas criticas

- **No conectar con APIs externas.** Esta API usa solo datos locales.
- **No crear migraciones EF Core.** Cambios de esquema via scripts SQL en `sql/`.
- **Todas las passwords de prueba son "12345".** No implementar hashing (es entorno de test).
- **Preguntar antes de actuar** si la documentacion no cubre el caso.
- **Compilar siempre** despues de cambios de codigo: `dotnet build backend/src/TajamarFaltas.Api/`
- **No borrar** `TajamarApiClientService` ni su registro sin confirmacion del usuario (existe por compatibilidad).
