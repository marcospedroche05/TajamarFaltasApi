using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Domain.Entities;

namespace TajamarFaltas.Application.Common;

public static class FaltaMappings
{
    public static MisFaltaDto ToMisFaltaDto(this Falta falta)
    {
        return new MisFaltaDto
        {
            Id = falta.Id,
            IdUsuario = falta.IdUsuario,
            IdCurso = falta.IdCurso,
            FechaIncidencia = falta.FechaIncidencia,
            TipoFalta = falta.TipoFalta switch
            {
                Domain.Enums.TipoFalta.Falta => "Falta",
                Domain.Enums.TipoFalta.Retraso => "Retraso",
                Domain.Enums.TipoFalta.SalidaDeAntes => "Salida de antes",
                _ => "Falta"
            },
            EsJustificada = falta.EsJustificada,
            Comentario = falta.Comentario
        };
    }
}