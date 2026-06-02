using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement;

public interface IMisFaltasQueryService
{
    Task<IReadOnlyList<MisFaltaDto>> GetMisFaltasAsync(int usuarioId, CancellationToken cancellationToken = default);
}