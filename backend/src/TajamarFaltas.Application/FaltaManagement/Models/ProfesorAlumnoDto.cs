namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class ProfesorAlumnoDto
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
