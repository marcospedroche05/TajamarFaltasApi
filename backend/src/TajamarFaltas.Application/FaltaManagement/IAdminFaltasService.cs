using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement
{
    public interface IAdminFaltasService
    {
        Task<IReadOnlyList<AdminFaltaDto>> GetAllFaltasAsync(CancellationToken cancellationToken = default);
        Task<bool> UpdateJustificacionAsync(int id, bool esJustificada, CancellationToken cancellationToken = default);
        Task<bool> EliminarFaltaAsync(int id, CancellationToken cancellationToken = default);
    }
}
