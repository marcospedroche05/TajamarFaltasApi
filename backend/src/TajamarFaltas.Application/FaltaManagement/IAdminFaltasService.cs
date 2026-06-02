using System.Collections.Generic;
using System.Threading.Tasks;
using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement
{
    public interface IAdminFaltasService
    {
        Task<IEnumerable<AdminFaltaDto>> GetAllFaltasAsync();
        Task<bool> UpdateJustificacionAsync(int id, bool esJustificada);
    }
}
