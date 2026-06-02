using TajamarFaltas.Application.Auth.Models;

namespace TajamarFaltas.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(Models.LoginRequest request, CancellationToken cancellationToken = default);
}
