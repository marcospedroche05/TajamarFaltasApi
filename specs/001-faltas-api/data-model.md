# Data Model: API de Gestión de Faltas Tajamar

## Entities

### Role

- `IdRole` (`tinyint`, primary key): Allowed values are 1, 2, and 3.
- `Rolename` (`nvarchar(50)`, unique): Human-readable role name.

Validation rules:
- Must contain exactly one of: Profesor, Alumno, Administrador.

### Usuario

- `Id` (`int`, primary key): Mirror identifier from the external system.
- `Nombre` (`nvarchar(100)`): Given name.
- `Apellidos` (`nvarchar(150)`): Surname(s).
- `Email` (`nvarchar(254)`, unique): Login identity.
- `EstadoUsuario` (`bit`): Indicates whether the account is active.
- `Imagen` (`nvarchar(500)`, nullable): Optional image URL or path.
- `IdRole` (`tinyint`, foreign key to Role): Associated role.

Validation rules:
- Email must be unique.
- Role must exist.
- Only active users can be authenticated locally.

### Curso

- `IdCurso` (`int`, primary key): Mirror identifier from the external system.
- `Nombre` (`nvarchar(150)`): Course name.
- `DuracionHoras` (`int`): Duration in hours.
- `Activo` (`bit`): Indicates whether the course is usable.

Validation rules:
- `DuracionHoras` must be greater than 0.
- Only active courses should be available for new absences.

### Falta

- `Id` (`int`, identity primary key): Local absence identifier.
- `IdUsuario` (`int`, foreign key to Usuario): Student or user associated with the absence.
- `IdCurso` (`int`, foreign key to Curso): Course where the absence occurred.
- `FechaIncidencia` (`datetime2(0)`): Exact timestamp of the incident.
- `TipoFalta` (`varchar(20)`): One of `Falta`, `Retraso`, or `Salida de antes`.
- `EsJustificada` (`bit`): Justification status.
- `Comentario` (`nvarchar(500)`, nullable): Optional explanation.

Validation rules:
- `IdUsuario` and `IdCurso` must reference existing mirror records.
- `TipoFalta` must match one of the three allowed values.
- `EsJustificada` defaults to false when the record is created.
- `FechaIncidencia` is required.

## Relationships

- One Role has many Usuarios.
- One Usuario can have many Faltas.
- One Curso can have many Faltas.

## State and Access Rules

- Alumno reads only own Faltas.
- Profesor reads Faltas scoped to the authorized course and can create new Faltas within that scope.
- Administrador reads and updates any Falta, including justification state.
