using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Entities;

namespace TajamarFaltas.Application.Common;

public static class ProfesorFaltaMappings
{
    public static ProfesorCursoDto ToProfesorCursoDto(this CursoMirror curso)
    {
        return new ProfesorCursoDto
        {
            IdCurso = curso.IdCurso,
            Nombre = curso.Nombre,
            DuracionHoras = curso.DuracionHoras
        };
    }

    public static ProfesorAlumnoDto ToProfesorAlumnoDto(this UsuarioMirror usuario)
    {
        return new ProfesorAlumnoDto
        {
            IdUsuario = usuario.Id,
            Nombre = usuario.Nombre,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email
        };
    }

    public static ProfesorFaltaDto ToProfesorFaltaDto(this Falta falta)
    {
        return new ProfesorFaltaDto
        {
            Id = falta.Id,
            IdUsuario = falta.IdUsuario,
            IdCurso = falta.IdCurso,
            FechaIncidencia = falta.FechaIncidencia,
            TipoFalta = falta.TipoFalta switch
            {
                Domain.Enums.TipoFalta.Falta => "Falta",
                Domain.Enums.TipoFalta.Retraso => "Retraso",
                Domain.Enums.TipoFalta.SalidaDeAntes => "Salida de antes",
                _ => "Falta"
            },
            EsJustificada = falta.EsJustificada,
            Comentario = falta.Comentario,
            NombreAlumno = falta.Usuario?.Nombre ?? string.Empty,
            ApellidosAlumno = falta.Usuario?.Apellidos ?? string.Empty
        };
    }
}