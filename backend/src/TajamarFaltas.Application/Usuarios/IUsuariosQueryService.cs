using TajamarFaltas.Application.Usuarios.Models;

namespace TajamarFaltas.Application.Usuarios;

public interface IUsuariosQueryService
{
    Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
