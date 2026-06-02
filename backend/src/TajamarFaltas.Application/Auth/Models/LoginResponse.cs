namespace TajamarFaltas.Application.Auth.Models;

public sealed class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public UserDto? User { get; set; }
}
