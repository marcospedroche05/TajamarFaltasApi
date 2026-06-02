-- =============================================================================
-- Script : 002_create_usuarioscursos.sql
-- Proposito: Crear la tabla UsuariosCursos (relacion N:M entre usuarios y cursos)
--            e insertar datos de seed con las asignaciones iniciales al curso 3430.
-- Idempotente: se puede ejecutar multiples veces sin error.
-- =============================================================================

USE ProyectoFaltas;
GO

-- -----------------------------------------------------------------------------
-- Crear tabla e insertar seed solo si la tabla aun no existe
-- -----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.UsuariosCursos', N'U') IS NULL
BEGIN
    -- Crear tabla
    CREATE TABLE dbo.UsuariosCursos
    (
        IdUsuario   int NOT NULL,
        IdCurso     int NOT NULL,

        CONSTRAINT PK_UsuariosCursos
            PRIMARY KEY (IdUsuario, IdCurso),

        CONSTRAINT FK_UsuariosCursos_UsuariosMirror
            FOREIGN KEY (IdUsuario) REFERENCES dbo.UsuariosMirror (Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

        CONSTRAINT FK_UsuariosCursos_CursosMirror
            FOREIGN KEY (IdCurso) REFERENCES dbo.CursosMirror (IdCurso)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
    );

    -- Insertar datos de seed
    -- Curso 3430: profesor (Id=31) + alumnos (Id=82..111, excluye 105)
    INSERT INTO dbo.UsuariosCursos (IdUsuario, IdCurso)
    VALUES
        -- Profesor
        (31,  3430),
        -- Alumnos
        (82,  3430),
        (83,  3430),
        (84,  3430),
        (85,  3430),
        (86,  3430),
        (87,  3430),
        (88,  3430),
        (89,  3430),
        (90,  3430),
        (91,  3430),
        (92,  3430),
        (93,  3430),
        (94,  3430),
        (95,  3430),
        (96,  3430),
        (97,  3430),
        (98,  3430),
        (99,  3430),
        (100, 3430),
        (101, 3430),
        (102, 3430),
        (103, 3430),
        (104, 3430),
        (106, 3430),
        (107, 3430),
        (108, 3430),
        (109, 3430),
        (110, 3430),
        (111, 3430);
END;
GO
