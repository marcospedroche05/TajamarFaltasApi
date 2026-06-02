# Feature Specification: API de Gestión de Faltas Tajamar

**Feature Branch**: `[001-faltas-api]`

**Created**: 2026-05-26

**Status**: Draft

**Input**: API de gestión de faltas para alumnos, profesores y administradores, con autenticación externa, persistencia local y reglas de acceso por rol.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Autenticación y acceso inicial (Priority: P1)

Como usuario autorizado, quiero iniciar sesión con mis credenciales de Tajamar y obtener acceso al sistema para poder usar las funciones permitidas según mi rol.

**Why this priority**: Sin autenticación no se puede aplicar control de acceso ni exponer ninguna consulta o acción sobre las faltas.

**Independent Test**: Puede probarse de forma aislada validando que un usuario con credenciales correctas obtiene acceso y que un usuario con credenciales incorrectas no lo consigue.

**Acceptance Scenarios**:

1. **Given** un usuario con credenciales válidas en la fuente externa, **When** inicia sesión, **Then** recibe acceso autenticado para continuar con acciones permitidas.
2. **Given** un usuario con credenciales inválidas, **When** intenta iniciar sesión, **Then** no recibe acceso y ve un error de autenticación.

---

### User Story 2 - Consulta de faltas propias del alumno (Priority: P2)

Como alumno, quiero consultar únicamente mis faltas para revisar incidencias, fechas, curso asociado y si están justificadas.

**Why this priority**: Es la consulta principal de lectura para el rol más restringido y permite validar el aislamiento de datos por usuario.

**Independent Test**: Puede probarse con un alumno autenticado comprobando que solo aparecen sus registros y ningún otro dato de terceros.

**Acceptance Scenarios**:

1. **Given** un alumno autenticado con faltas registradas, **When** consulta sus faltas, **Then** obtiene solo los registros asociados a su usuario.
2. **Given** un alumno autenticado sin faltas registradas, **When** consulta sus faltas, **Then** obtiene una respuesta vacía sin errores.

---

### User Story 3 - Gestión de faltas del profesor (Priority: P3)

Como profesor, quiero consultar las faltas de mi curso y registrar nuevas incidencias para mantener actualizado el seguimiento del grupo que gestiono.

**Why this priority**: El profesor necesita lectura y alta de faltas sobre el ámbito que tiene autorizado, que es una de las funciones operativas del sistema.

**Independent Test**: Puede probarse con un profesor autenticado verificando que solo ve las faltas permitidas y que puede crear una falta válida para un alumno autorizado.

**Acceptance Scenarios**:

1. **Given** un profesor autenticado con permisos sobre un curso, **When** consulta las faltas de ese curso, **Then** solo ve las incidencias autorizadas para ese ámbito.
2. **Given** un profesor autenticado y un alumno válido del curso autorizado, **When** registra una nueva falta válida, **Then** la falta queda almacenada y disponible en consultas posteriores.
3. **Given** un profesor autenticado intentando operar sobre un alumno o curso no autorizado, **When** realiza la acción, **Then** la solicitud es rechazada.

---

### User Story 4 - Supervisión total del administrador (Priority: P4)

Como administrador, quiero consultar todas las faltas y marcar si una incidencia está justificada para poder supervisar y corregir el registro global.

**Why this priority**: Es la función de mayor alcance, pero depende de que la autenticación y la consulta básica ya estén funcionando correctamente.

**Independent Test**: Puede probarse con un administrador autenticado verificando lectura global y actualización del estado de justificación sobre un registro existente.

**Acceptance Scenarios**:

1. **Given** un administrador autenticado, **When** consulta las faltas, **Then** puede ver todos los registros del sistema.
2. **Given** un administrador autenticado y una falta existente, **When** marca la incidencia como justificada o no justificada, **Then** el cambio queda reflejado en la información devuelta por consultas posteriores.

### Edge Cases

- Qué ocurre cuando el servicio externo de autenticación no está disponible o rechaza el acceso.
- Qué ocurre cuando un usuario intenta consultar o modificar faltas fuera de su rol o ámbito autorizado.
- Qué ocurre cuando se intenta registrar una falta con un alumno, un curso o un tipo de falta inexistente.
- Qué ocurre cuando se intenta justificar una falta que no existe.
- Qué ocurre cuando una consulta no tiene resultados porque no hay faltas registradas para ese usuario o curso.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: El sistema debe permitir que un usuario inicie sesión con credenciales reconocidas por la fuente externa y obtenga acceso autenticado para operar según su rol.
- **FR-002**: El sistema debe bloquear el acceso a cualquier operación protegida cuando el usuario no esté autenticado o su autenticación no sea válida.
- **FR-003**: El sistema debe permitir que un alumno consulte únicamente las faltas asociadas a su propio usuario.
- **FR-004**: El sistema debe permitir que un profesor consulte únicamente las faltas de los cursos que tiene autorizados.
- **FR-005**: El sistema debe permitir que un profesor registre una nueva falta para un alumno dentro de un curso autorizado.
- **FR-006**: El sistema debe permitir que un administrador consulte todas las faltas registradas en el sistema.
- **FR-007**: El sistema debe permitir que un administrador cambie el estado de justificada o no justificada de una falta existente.
- **FR-008**: El sistema debe validar que toda falta registrada esté asociada a un alumno existente, a un curso existente y a un tipo de falta permitido.
- **FR-009**: El sistema debe conservar para cada falta la fecha de incidencia, el comentario opcional y el estado de justificación.
- **FR-010**: El sistema debe respetar los límites de acceso por rol y rechazar cualquier intento de leer o modificar datos fuera del ámbito permitido.
- **FR-011**: El sistema debe reflejar en las consultas posteriores cualquier alta o actualización realizada sobre una falta existente.

### Key Entities *(include if feature involves data)*

- **Roles**: Catálogo de permisos del sistema; distingue entre Profesor, Alumno y Administrador.
- **Usuarios**: Personas autorizadas en el sistema; contiene identidad básica, estado, correo y rol asociado.
- **Cursos**: Catálogo de cursos disponibles; representa la unidad académica sobre la que se consultan y registran faltas.
- **Faltas**: Registro principal de incidencias; vincula un usuario con un curso, una fecha, un tipo de falta, un estado de justificación y un comentario opcional.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: El 100% de los usuarios con credenciales válidas completan el acceso autenticado y pueden continuar con acciones permitidas por su rol.
- **SC-002**: El 100% de las consultas de alumnos muestran solo faltas del propio alumno autenticado, sin exponer datos de otros usuarios.
- **SC-003**: Al menos el 95% de los profesores autorizados pueden consultar sus faltas y registrar una nueva incidencia válida sin intervención manual adicional.
- **SC-004**: El 100% de los administradores autorizados pueden cambiar el estado de justificación de una falta existente y ver el cambio reflejado en consultas posteriores.
- **SC-005**: El 95% de las operaciones habituales de consulta o registro se completan en menos de 5 segundos para el usuario.

## Assumptions

- La API externa de Tajamar aporta la autenticación y la información necesaria para identificar el rol del usuario autenticado.
- La relación entre profesor y curso autorizado se obtiene desde la información externa o de las credenciales de acceso, ya que no existe una tabla local específica para esa asignación.
- El primer alcance cubre autenticación, consulta y gestión de faltas; la administración de usuarios, roles y cursos queda fuera de esta entrega.
- El frontend Angular existente consumirá esta API y no forma parte de esta especificación.
- Los nombres, relaciones y restricciones del script SQL proporcionado se consideran la definición definitiva del modelo local.
