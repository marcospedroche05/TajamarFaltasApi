using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.Common;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Enums;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement;

public sealed class MisFaltasQueryService : IMisFaltasQueryService
{
    private readonly TajamarDbContext _db;

    public MisFaltasQueryService(TajamarDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MisFaltaDto>> GetMisFaltasAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await _db.Faltas
            .AsNoTracking()
            .Where(falta => falta.IdUsuario == usuarioId)
            .OrderByDescending(falta => falta.FechaIncidencia)
            .ThenByDescending(falta => falta.Id)
            .Select(falta => falta.ToMisFaltaDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AlumnoCursoDto>> GetMisCursosAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await _db.UsuariosCursos
            .AsNoTracking()
            .Where(uc => uc.IdUsuario == usuarioId && uc.Curso!.Activo)
            .Select(uc => uc.Curso!.ToAlumnoCursoDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ResumenAsistenciaDto>> GetResumenAsistenciaAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        var cursos = await _db.UsuariosCursos
            .AsNoTracking()
            .Where(uc => uc.IdUsuario == usuarioId && uc.Curso!.Activo)
            .Select(uc => new { uc.Curso!.IdCurso, uc.Curso.Nombre, uc.Curso.DuracionHoras })
            .ToListAsync(cancellationToken);

        var idsCursos = cursos.Select(c => c.IdCurso).ToList();

        var horasFaltaPorCurso = await _db.Faltas
            .AsNoTracking()
            .Where(f => f.IdUsuario == usuarioId
                        && idsCursos.Contains(f.IdCurso)
                        && f.TipoFalta == TipoFalta.Falta)
            .GroupBy(f => f.IdCurso)
            .Select(g => new { IdCurso = g.Key, HorasFalta = g.Count() })
            .ToListAsync(cancellationToken);

        var faltasPorCurso = horasFaltaPorCurso.ToDictionary(x => x.IdCurso, x => x.HorasFalta);

        return cursos.Select(curso =>
        {
            var horasFalta = faltasPorCurso.GetValueOrDefault(curso.IdCurso, 0);
            var porcentaje = curso.DuracionHoras > 0
                ? Math.Round(((curso.DuracionHoras - horasFalta) / (double)curso.DuracionHoras) * 100, 1)
                : 0.0;

            return new ResumenAsistenciaDto
            {
                IdCurso = curso.IdCurso,
                NombreCurso = curso.Nombre,
                TotalHoras = curso.DuracionHoras,
                HorasFalta = horasFalta,
                PorcentajeAsistencia = porcentaje
            };
        }).ToList();
    }
}