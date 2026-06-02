namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class ProfesorCursoDto
{
    public int IdCurso { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int DuracionHoras { get; set; }
}
