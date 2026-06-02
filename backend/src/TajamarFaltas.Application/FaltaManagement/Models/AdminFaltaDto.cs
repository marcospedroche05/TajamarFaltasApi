using System;

namespace TajamarFaltas.Application.FaltaManagement.Models
{
    public class AdminFaltaDto
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; } = string.Empty;
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public bool EsJustificada { get; set; }
        public string? Observaciones { get; set; }
    }
}
