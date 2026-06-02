using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.Common;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.FaltaManagement;

public sealed class MisFaltasQueryService : IMisFaltasQueryService
{
    private readonly TajamarDbContext _db;

    public MisFaltasQueryService(TajamarDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MisFaltaDto>> GetMisFaltasAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await _db.Faltas
            .AsNoTracking()
            .Where(falta => falta.IdUsuario == usuarioId)
            .OrderByDescending(falta => falta.FechaIncidencia)
            .ThenByDescending(falta => falta.Id)
            .Select(falta => falta.ToMisFaltaDto())
            .ToListAsync(cancellationToken);
    }
}