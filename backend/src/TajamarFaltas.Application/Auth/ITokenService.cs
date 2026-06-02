using TajamarFaltas.Domain.Entities;

namespace TajamarFaltas.Application.Auth;

public interface ITokenService
{
    Task<(string AccessToken, int ExpiresIn)> GenerateTokenAsync(UsuarioMirror user, CancellationToken cancellationToken = default);
}
