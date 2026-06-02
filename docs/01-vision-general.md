# Vision general del proyecto

## Contexto

El centro educativo Tajamar gestionaba las faltas de asistencia mediante hojas de Excel individuales por profesor y clase. Este sistema presentaba dos problemas principales:

1. Los alumnos no podian consultar sus propias faltas.
2. No existia un calculo automatizado del porcentaje de asistencia.

## Objetivo

Sustituir el sistema de Excel por una aplicacion web compuesta por:

- **Frontend**: aplicacion Angular que conectara en produccion con la API externa de Tajamar alojada en Azure.
- **Backend (este proyecto)**: API REST en .NET 8 / C# que sirve como **entorno de pruebas** para desarrollar y validar el frontend antes de integrarlo con la API real.

## Alcance de esta API

- Gestionar faltas, retrasos y salidas anticipadas de alumnos.
- Proporcionar endpoints diferenciados por rol: alumno, profesor y administrador.
- Autenticacion local mediante JWT.
- Base de datos SQL Server local con datos de prueba precargados.

> **Nota**: Esta API no conecta con la API externa de Tajamar. Los datos de usuarios y cursos se almacenan localmente en tablas "Mirror".

## Documentos relacionados

- [Arquitectura](02-arquitectura.md)
- [Roles y permisos](03-roles-y-permisos.md)
- [Modelo de datos](04-modelo-de-datos.md)
- [Endpoints](05-endpoints.md)
