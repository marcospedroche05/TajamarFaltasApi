using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Application.FaltaManagement;

public interface IProfesorFaltasService
{
    Task<IReadOnlyList<ProfesorFaltaDto>?> GetFaltasPorCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken = default);

    Task<ProfesorFaltaDto?> CrearFaltaAsync(int profesorId, ProfesorFaltaRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfesorCursoDto>> GetMisCursosAsync(int profesorId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfesorAlumnoDto>?> GetAlumnosDeCursoAsync(int profesorId, int idCurso, CancellationToken cancellationToken = default);

    Task<bool?> EliminarFaltaAsync(int profesorId, int idFalta, CancellationToken cancellationToken = default);
}