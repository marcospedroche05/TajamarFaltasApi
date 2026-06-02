using System.ComponentModel.DataAnnotations;

namespace TajamarFaltas.Application.Auth.Models;

public sealed class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Password { get; set; } = string.Empty;
}
