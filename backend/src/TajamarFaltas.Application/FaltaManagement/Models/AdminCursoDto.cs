namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class AdminCursoDto
{
    public int IdCurso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int DuracionHoras { get; set; }
    public bool Activo { get; set; }
}
