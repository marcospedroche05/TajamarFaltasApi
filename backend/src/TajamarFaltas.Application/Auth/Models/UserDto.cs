namespace TajamarFaltas.Application.Auth.Models;

public sealed class UserDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
