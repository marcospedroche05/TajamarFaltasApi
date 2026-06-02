namespace TajamarFaltas.Domain.Entities;

public sealed class UsuarioCurso
{
    public int IdUsuario { get; set; }
    public int IdCurso { get; set; }
    public UsuarioMirror? Usuario { get; set; }
    public CursoMirror? Curso { get; set; }
}
