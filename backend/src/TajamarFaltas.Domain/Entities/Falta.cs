using TajamarFaltas.Domain.Enums;

namespace TajamarFaltas.Domain.Entities;

public sealed class Falta
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCurso { get; set; }

    public DateTime FechaIncidencia { get; set; }

    public TipoFalta TipoFalta { get; set; }

    public bool EsJustificada { get; set; }

    public string? Comentario { get; set; }

    public UsuarioMirror? Usuario { get; set; }

    public CursoMirror? Curso { get; set; }
}