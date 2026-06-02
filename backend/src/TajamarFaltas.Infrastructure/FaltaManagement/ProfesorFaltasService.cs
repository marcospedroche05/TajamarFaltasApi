using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.Common;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Entities;
using TajamarFaltas.Domain.Enums;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement;

public sealed class ProfesorFaltasService : IProfesorFaltasService
{
    private readonly TajamarDbContext _db;

    public ProfesorFaltasService(TajamarDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProfesorFaltaDto>?> GetFaltasPorCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken = default)
    {
        var autorizado = await CanProfesorAccessCursoAsync(profesorId, idCurso, cancellationToken);
        if (!autorizado)
        {
            return null;
        }

        return await _db.Faltas
            .AsNoTracking()
            .Where(falta => falta.IdCurso == idCurso)
            .OrderByDescending(falta => falta.FechaIncidencia)
            .ThenByDescending(falta => falta.Id)
            .Select(falta => falta.ToProfesorFaltaDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfesorFaltaDto?> CrearFaltaAsync(int profesorId, ProfesorFaltaRequest request, CancellationToken cancellationToken = default)
    {
        var autorizado = await CanProfesorAccessCursoAsync(profesorId, request.IdCurso, cancellationToken);
        if (!autorizado)
        {
            return null;
        }

        var alumno = await _db.UsuariosMirror
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == request.IdUsuario, cancellationToken);

        if (alumno is null || alumno.IdRole != RoleType.Alumno || !alumno.EstadoUsuario)
        {
            return null;
        }

        var cursoExiste = await _db.CursosMirror
            .AsNoTracking()
            .AnyAsync(curso => curso.IdCurso == request.IdCurso && curso.Activo, cancellationToken);

        if (!cursoExiste)
        {
            return null;
        }

        if (!Enum.TryParse<TipoFalta>(request.TipoFalta.Replace(" ", string.Empty), ignoreCase: true, out var tipoFalta)
            && !TryMapTipoFalta(request.TipoFalta, out tipoFalta))
        {
            return null;
        }

        var falta = new Falta
        {
            IdUsuario = request.IdUsuario,
            IdCurso = request.IdCurso,
            FechaIncidencia = request.FechaIncidencia,
            TipoFalta = tipoFalta,
            EsJustificada = false,
            Comentario = request.Comentario
        };

        _db.Faltas.Add(falta);
        await _db.SaveChangesAsync(cancellationToken);

        return falta.ToProfesorFaltaDto();
    }

    private async Task<bool> CanProfesorAccessCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken)
    {
        var profesor = await _db.UsuariosMirror
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == profesorId, cancellationToken);

        if (profesor is null || profesor.IdRole != RoleType.Profesor || !profesor.EstadoUsuario)
        {
            return false;
        }

        return await _db.CursosMirror
            .AsNoTracking()
            .AnyAsync(curso => curso.IdCurso == idCurso && curso.Activo, cancellationToken);
    }

    private static bool TryMapTipoFalta(string value, out TipoFalta tipoFalta)
    {
        tipoFalta = TipoFalta.Falta;

        return value.Trim().ToLowerInvariant() switch
        {
            "falta" => Set(out tipoFalta, TipoFalta.Falta),
            "retraso" => Set(out tipoFalta, TipoFalta.Retraso),
            "salidadedeantes" => Set(out tipoFalta, TipoFalta.SalidaDeAntes),
            "salida de antes" => Set(out tipoFalta, TipoFalta.SalidaDeAntes),
            _ => false
        };
    }

    private static bool Set(out TipoFalta tipoFalta, TipoFalta value)
    {
        tipoFalta = value;
        return true;
    }
}