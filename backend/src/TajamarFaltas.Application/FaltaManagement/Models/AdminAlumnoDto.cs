namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class AdminAlumnoDto
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EstadoUsuario { get; set; }
}
