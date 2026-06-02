using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement;

public interface IMisFaltasQueryService
{
    Task<IReadOnlyList<MisFaltaDto>> GetMisFaltasAsync(int usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlumnoCursoDto>> GetMisCursosAsync(int usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ResumenAsistenciaDto>> GetResumenAsistenciaAsync(int usuarioId, CancellationToken cancellationToken = default);
}