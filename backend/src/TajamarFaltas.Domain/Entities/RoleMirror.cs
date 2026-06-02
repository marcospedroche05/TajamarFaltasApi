using TajamarFaltas.Domain.Enums;

namespace TajamarFaltas.Domain.Entities;

public sealed class RoleMirror
{
    public RoleType IdRole { get; set; }

    public string Rolename { get; set; } = string.Empty;

    public ICollection<UsuarioMirror> Usuarios { get; set; } = new List<UsuarioMirror>();
}