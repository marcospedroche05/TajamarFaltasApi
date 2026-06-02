using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Entities;

namespace TajamarFaltas.Application.Common;

public static class AlumnoMappings
{
    public static AlumnoCursoDto ToAlumnoCursoDto(this CursoMirror curso)
    {
        return new AlumnoCursoDto
        {
            IdCurso = curso.IdCurso,
            Nombre = curso.Nombre,
            DuracionHoras = curso.DuracionHoras
        };
    }
}
