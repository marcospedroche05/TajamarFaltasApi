using System.ComponentModel.DataAnnotations;

namespace TajamarFaltas.Application.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Required]
    public string SigningKey { get; set; } = string.Empty;

    public int AccessTokenMinutes { get; set; } = 120;
}