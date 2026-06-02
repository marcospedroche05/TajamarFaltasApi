using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TajamarFaltas.Application.Auth;
using TajamarFaltas.Application.Auth.Models;
using TajamarFaltas.Domain.Entities;

namespace TajamarFaltas.Infrastructure.Auth;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<(string AccessToken, int ExpiresIn)> GenerateTokenAsync(UsuarioMirror user, CancellationToken cancellationToken = default)
    {
        var signingKey = string.IsNullOrWhiteSpace(_options.SigningKey)
            ? "development-signing-key-development-signing-key"
            : _options.SigningKey;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, string.Join(' ', user.Nombre, user.Apellidos).Trim()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.IdRole.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var expiresIn = (int)TimeSpan.FromMinutes(_options.AccessTokenMinutes).TotalSeconds;

        return Task.FromResult((tokenString, expiresIn));
    }
}
