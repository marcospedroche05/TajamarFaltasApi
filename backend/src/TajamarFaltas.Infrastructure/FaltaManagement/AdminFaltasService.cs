using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement
{
    public sealed class AdminFaltasService : IAdminFaltasService
    {
        private readonly TajamarDbContext _db;

        public AdminFaltasService(TajamarDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<AdminFaltaDto>> GetAllFaltasAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Faltas
                .AsNoTracking()
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

        public async Task<bool> UpdateJustificacionAsync(int id, bool esJustificada, CancellationToken cancellationToken = default)
        {
            var falta = await _db.Faltas.FindAsync(new object[] { id }, cancellationToken);
            if (falta == null) return false;

            falta.EsJustificada = esJustificada;
            _db.Faltas.Update(falta);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> EliminarFaltaAsync(int id, CancellationToken cancellationToken = default)
        {
            var falta = await _db.Faltas.FindAsync(new object[] { id }, cancellationToken);
            if (falta == null) return false;

            _db.Faltas.Remove(falta);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
