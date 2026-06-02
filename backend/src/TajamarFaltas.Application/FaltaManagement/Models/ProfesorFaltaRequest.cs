using System.ComponentModel.DataAnnotations;

namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class ProfesorFaltaRequest
{
    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdCurso { get; set; }

    [Required]
    public DateTime FechaIncidencia { get; set; }

    [Required]
    public string TipoFalta { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Comentario { get; set; }
}