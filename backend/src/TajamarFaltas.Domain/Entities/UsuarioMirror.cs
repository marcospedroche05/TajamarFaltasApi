using TajamarFaltas.Domain.Enums;

namespace TajamarFaltas.Domain.Entities;

public sealed class UsuarioMirror
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool EstadoUsuario { get; set; }

    public string? Imagen { get; set; }

    public RoleType IdRole { get; set; }

    public RoleMirror? Role { get; set; }

    public ICollection<Falta> Faltas { get; set; } = new List<Falta>();
}