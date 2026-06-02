namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class ProfesorFaltaDto
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCurso { get; set; }

    public DateTime FechaIncidencia { get; set; }

    public string TipoFalta { get; set; } = string.Empty;

    public bool EsJustificada { get; set; }

    public string? Comentario { get; set; }
}