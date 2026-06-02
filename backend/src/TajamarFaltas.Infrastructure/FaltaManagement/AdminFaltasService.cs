using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement
{
    public class AdminFaltasService : IAdminFaltasService
    {
        private readonly TajamarDbContext _db;

        public AdminFaltasService(TajamarDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AdminFaltaDto>> GetAllFaltasAsync()
        {
            var faltas = await _db.Faltas
                .Include(f => f.Usuario)
                .Include(f => f.Curso)
                .OrderByDescending(f => f.FechaIncidencia)
                .ToListAsync();

            return faltas.Select(f => new AdminFaltaDto
            {
                Id = f.Id,
                IdAlumno = f.IdUsuario,
                NombreAlumno = f.Usuario != null ? (f.Usuario.Nombre ?? f.Usuario.Email) : string.Empty,
                IdCurso = f.IdCurso,
                NombreCurso = f.Curso != null ? f.Curso.Nombre : string.Empty,
                Fecha = f.FechaIncidencia,
                Tipo = f.TipoFalta.ToString(),
                EsJustificada = f.EsJustificada,
                Observaciones = f.Comentario
            }).ToList();
        }

        public async Task<bool> UpdateJustificacionAsync(int id, bool esJustificada)
        {
            var falta = await _db.Faltas.FindAsync(id);
            if (falta == null) return false;

            falta.EsJustificada = esJustificada;
            _db.Faltas.Update(falta);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
