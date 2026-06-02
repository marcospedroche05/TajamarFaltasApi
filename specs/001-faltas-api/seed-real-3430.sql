SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DELETE FROM dbo.Faltas;
DELETE FROM dbo.UsuariosMirror;
DELETE FROM dbo.CursosMirror;
DELETE FROM dbo.RolesMirror;

INSERT INTO dbo.RolesMirror (IdRole, Rolename)
VALUES
    (1, N'Profesor'),
    (2, N'Alumno'),
    (3, N'Administrador');

INSERT INTO dbo.CursosMirror (IdCurso, Nombre, DuracionHoras, Activo)
VALUES
    (3430, N'Master Desarrollo Apps Cloud 2025-2026', 960, 1);

INSERT INTO dbo.UsuariosMirror (Id, Nombre, Apellidos, Email, EstadoUsuario, Imagen, IdRole)
VALUES
    (31, N'Paco', N'Garcia Serrano', N'paco.garcia.serrano@tajamar365.com', 1, N'https://apicharlasalumnotajamar.azurewebsites.net/images/users/31_user.jpg', 1),
    (32, N'Admin', N'Admin', N'admin@tajamar365.com', 1, N'nouser.png', 3),
    (82, N'María', N'Miguel Tolosana', N'maria.miguel@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (83, N'Alejandro', N'Ruiz García', N'alejandro.ruizgarcia@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (84, N'Alejandro', N'Cobo Marcos', N'alejandro.cobo@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (85, N'Luis Miguel', N'Cañizares Diaz', N'Luismiguel.canizares@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (86, N'Alberto', N'Barbacid', N'alberto.barbacid@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (87, N'Alonso', N'García Martín', N'alonso.garcia@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (88, N'Diego', N'Cardona Hernandez', N'diego.cardona@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (89, N'Adrian', N'Jacek', N'adrian.jacek@tajamar365.com', 1, N'https://apicharlasalumnotajamar.azurewebsites.net/images/users/89_user.png', 2),
    (90, N'Héctor', N'Gil Fuertes', N'hector.gil@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (91, N'Julio Alejandro', N'Ordoñez Rimacuna', N'julioalejandro.ordonez@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (92, N'Álvaro', N'Casco Valero', N'alvaro.casco@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (93, N'Angel', N'Pinto Diaz', N'angel.pinto@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (94, N'Raúl', N'García Muñoz', N'raul.garciamunoz@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (95, N'Diego', N'Pérez Gregorio', N'diego.perez@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (96, N'Jorge', N'Rodríguez Alonso', N'jorge.rodriguezalonso@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (97, N'Alejandro', N'Navarro', N'alejandro.navarro@tajamar365.com', 1, N'https://apicharlasalumnotajamar.azurewebsites.net/images/users/97_user.jfif', 2),
    (98, N'Jose Antonio', N'López Pachón', N'joseantonio.lopez@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (99, N'Marta', N'Quirós Martín-Portugués', N'marta.quiros@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (100, N'Pablo', N'Gonzalo Lucas', N'pablo.gonzalo@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (101, N'Juan', N'Solís Torrijos', N'juan.solis@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (102, N'Marcos', N'Pedroche Pérez', N'marcos.pedroche@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (103, N'Alberto', N'Rodriguez-Rey', N'alberto.rodriguez-rey@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (104, N'Kevin Sebastián', N'Bayas Sarzosa', N'kevin.bayas@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (106, N'Alejandro', N'Cánovas López', N'alejandro.canovas@tajamar365.com', 1, N'https://apicharlasalumnotajamar.azurewebsites.net/images/users/106_user.png', 2),
    (107, N'Javier', N'Alonso Mansilla', N'javier.alonsomansilla@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (108, N'Asil', N'Galan', N'asil.galan@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (109, N'Alejandro', N'Amores Fraile', N'alejandro.amores@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (110, N'Ivan', N'Vazquez', N'ivan.vazquez@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2),
    (111, N'Alumno', N'Test', N'alumnotest@tajamar365.com', 1, N'https://cdn.pixabay.com/photo/2017/11/10/05/48/user-2935527_640.png', 2);

INSERT INTO dbo.Faltas (IdUsuario, IdCurso, FechaIncidencia, TipoFalta, EsJustificada, Comentario)
VALUES
    (82, 3430, '2025-10-07T09:15:00', 'Falta', 0, N'No asistió por visita médica.'),
    (83, 3430, '2025-10-09T10:05:00', 'Retraso', 1, N'Llegó tarde por tráfico en M30.'),
    (84, 3430, '2025-10-15T08:55:00', 'Falta', 0, N'No compareció a primera hora.'),
    (85, 3430, '2025-10-21T12:40:00', 'Salida de antes', 0, N'Se ausentó por una entrevista de trabajo.'),
    (87, 3430, '2025-11-04T09:00:00', 'Falta', 1, N'Ausencia justificada por enfermedad.'),
    (89, 3430, '2025-11-11T09:20:00', 'Retraso', 1, N'Retraso por problema con el transporte.'),
    (90, 3430, '2025-11-19T08:50:00', 'Falta', 0, N'No asistió sin aviso.'),
    (95, 3430, '2025-12-02T14:10:00', 'Salida de antes', 1, N'Salida anticipada por cita médica.'),
    (100, 3430, '2026-01-14T09:00:00', 'Falta', 0, N'Ausencia en sesión de prácticas.'),
    (106, 3430, '2026-01-28T10:10:00', 'Retraso', 0, N'Llegó tarde por avería del coche.'),
    (111, 3430, '2026-02-10T09:05:00', 'Falta', 1, N'Falta justificada por enfermedad.');

COMMIT TRANSACTION;
