using Microsoft.EntityFrameworkCore;
using TajamarFaltas.Application.Auth;
using TajamarFaltas.Application.Auth.Models;
using TajamarFaltas.Infrastructure.Persistence;

namespace TajamarFaltas.Infrastructure.Auth;

public sealed class AuthService : IAuthService
{
    private readonly TajamarDbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(TajamarDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var usuario = await _db.UsuariosMirror
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (usuario is null || !usuario.EstadoUsuario)
        {
            return null;
        }

        if (usuario.Password != request.Password)
        {
            return null;
        }

        var (token, expiresIn) = await _tokenService.GenerateTokenAsync(usuario, cancellationToken);

        return new LoginResponse
        {
            AccessToken = token,
            ExpiresIn = expiresIn,
            User = new UserDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Role = usuario.IdRole.ToString()
            }
        };
    }
}
