using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Enums;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement
{
    public sealed class AdminCursosService : IAdminCursosService
    {
        private readonly TajamarDbContext _db;

        public AdminCursosService(TajamarDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<AdminCursoDto>> GetAllCursosAsync(CancellationToken cancellationToken = default)
        {
            return await _db.CursosMirror
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .Select(c => new AdminCursoDto
                {
                    IdCurso = c.IdCurso,
                    Nombre = c.Nombre,
                    DuracionHoras = c.DuracionHoras,
                    Activo = c.Activo
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AdminAlumnoDto>> GetAlumnosDeCursoAsync(int idCurso, CancellationToken cancellationToken = default)
        {
            return await _db.UsuariosCursos
                .AsNoTracking()
                .Where(uc => uc.IdCurso == idCurso && uc.Usuario!.IdRole == RoleType.Alumno)
                .Select(uc => new AdminAlumnoDto
                {
                    IdUsuario = uc.Usuario!.Id,
                    Nombre = uc.Usuario.Nombre,
                    Apellidos = uc.Usuario.Apellidos,
                    Email = uc.Usuario.Email,
                    EstadoUsuario = uc.Usuario.EstadoUsuario
                })
                .OrderBy(a => a.Apellidos)
                .ThenBy(a => a.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AdminFaltaDto>> GetFaltasDeCursoAsync(int idCurso, CancellationToken cancellationToken = default)
        {
            return await _db.Faltas
                .AsNoTracking()
                .Where(f => f.IdCurso == idCurso)
                .OrderByDescending(f => f.FechaIncidencia)
                .Select(f => new AdminFaltaDto
                {
                    Id = f.Id,
                    IdAlumno = f.IdUsuario,
                    NombreAlumno = f.Usuario != null ? $"{f.Usuario.Nombre} {f.Usuario.Apellidos}".Trim() : string.Empty,
                    IdCurso = f.IdCurso,
                    NombreCurso = f.Curso != null ? f.Curso.Nombre : string.Empty,
                    Fecha = f.FechaIncidencia,
                    Tipo = f.TipoFalta.ToString(),
                    EsJustificada = f.EsJustificada,
                    Observaciones = f.Comentario
                })
                .ToListAsync(cancellationToken);
        }
    }
}
