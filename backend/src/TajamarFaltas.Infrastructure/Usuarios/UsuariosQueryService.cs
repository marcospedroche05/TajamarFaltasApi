using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.Usuarios;
using TajamarFaltas.Application.Usuarios.Models;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.Usuarios;

public sealed class UsuariosQueryService : IUsuariosQueryService
{
    private readonly TajamarDbContext _db;

    public UsuariosQueryService(TajamarDbContext db)
    {
        _db = db;
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _db.UsuariosMirror
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (usuario is null)
        {
            return null;
        }

        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Role = usuario.IdRole.ToString(),
            EstadoUsuario = usuario.EstadoUsuario
        };
    }
}
