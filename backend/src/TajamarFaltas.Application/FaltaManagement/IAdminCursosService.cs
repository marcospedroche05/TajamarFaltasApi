using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement
{
    public interface IAdminCursosService
    {
        Task<IReadOnlyList<AdminCursoDto>> GetAllCursosAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AdminAlumnoDto>> GetAlumnosDeCursoAsync(int idCurso, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AdminFaltaDto>> GetFaltasDeCursoAsync(int idCurso, CancellationToken cancellationToken = default);
    }
}
