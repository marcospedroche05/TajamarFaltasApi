namespace TajamarFaltas.Domain.Entities;

public sealed class CursoMirror
{
    public int IdCurso { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int DuracionHoras { get; set; }

    public bool Activo { get; set; }

    public ICollection<Falta> Faltas { get; set; } = new List<Falta>();
}