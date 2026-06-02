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

        var faltas = await _db.Faltas
            .AsNoTracking()
            .Include(f => f.Usuario)
            .Where(f => f.IdCurso == idCurso)
            .OrderByDescending(f => f.FechaIncidencia)
            .ThenByDescending(f => f.Id)
            .ToListAsync(cancellationToken);

        return faltas.Select(f => f.ToProfesorFaltaDto()).ToList();
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

    public async Task<IReadOnlyList<ProfesorCursoDto>> GetMisCursosAsync(int profesorId, CancellationToken cancellationToken = default)
    {
        return await _db.UsuariosCursos
            .AsNoTracking()
            .Where(uc => uc.IdUsuario == profesorId
                && uc.Usuario!.IdRole == RoleType.Profesor
                && uc.Usuario.EstadoUsuario
                && uc.Curso!.Activo)
            .Select(uc => new ProfesorCursoDto
            {
                IdCurso = uc.Curso!.IdCurso,
                Nombre = uc.Curso.Nombre,
                DuracionHoras = uc.Curso.DuracionHoras
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfesorAlumnoDto>?> GetAlumnosDeCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken = default)
    {
        var autorizado = await CanProfesorAccessCursoAsync(profesorId, idCurso, cancellationToken);
        if (!autorizado)
        {
            return null;
        }

        return await _db.UsuariosCursos
            .AsNoTracking()
            .Where(uc => uc.IdCurso == idCurso
                && uc.Usuario!.IdRole == RoleType.Alumno
                && uc.Usuario.EstadoUsuario)
            .Select(uc => new ProfesorAlumnoDto
            {
                IdUsuario = uc.Usuario!.Id,
                Nombre = uc.Usuario.Nombre,
                Apellidos = uc.Usuario.Apellidos,
                Email = uc.Usuario.Email
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool?> EliminarFaltaAsync(int profesorId, int idFalta, CancellationToken cancellationToken = default)
    {
        var falta = await _db.Faltas
            .FirstOrDefaultAsync(f => f.Id == idFalta, cancellationToken);

        if (falta is null)
        {
            return false;
        }

        var autorizado = await CanProfesorAccessCursoAsync(profesorId, falta.IdCurso, cancellationToken);
        if (!autorizado)
        {
            return null;
        }

        _db.Faltas.Remove(falta);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<bool> CanProfesorAccessCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken)
    {
        return await _db.UsuariosCursos
            .AsNoTracking()
            .AnyAsync(uc => uc.IdUsuario == profesorId
                && uc.IdCurso == idCurso
                && uc.Usuario!.IdRole == RoleType.Profesor
                && uc.Usuario.EstadoUsuario
                && uc.Curso!.Activo,
                cancellationToken);
    }

    private static bool TryMapTipoFalta(string value, out TipoFalta tipoFalta)
    {
        tipoFalta = TipoFalta.Falta;

        return value.Trim().ToLowerInvariant() switch
        {
            "falta" => Set(out tipoFalta, TipoFalta.Falta),
            "retraso" => Set(out tipoFalta, TipoFalta.Retraso),
            "salidadeantes" => Set(out tipoFalta, TipoFalta.SalidaDeAntes),
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