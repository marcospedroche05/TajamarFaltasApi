namespace TajamarFaltas.Application.FaltaManagement.Models;

public sealed class ResumenAsistenciaDto
{
    public int IdCurso { get; set; }

    public string NombreCurso { get; set; } = string.Empty;

    public int TotalHoras { get; set; }

    public int HorasFalta { get; set; }

    public double PorcentajeAsistencia { get; set; }
}
