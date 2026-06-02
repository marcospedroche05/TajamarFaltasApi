using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Api.Authorization;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Api.Controllers;

[ApiController]
[Route("api/profesor")]
[Authorize(Policy = PolicyNames.ProfesorOnly)]
public class ProfesorFaltasController : ControllerBase
{
    private readonly IProfesorFaltasService _profesorFaltasService;

    public ProfesorFaltasController(IProfesorFaltasService profesorFaltasService)
    {
        _profesorFaltasService = profesorFaltasService;
    }

    [HttpGet("cursos/{idCurso:int}/faltas")]
    public async Task<ActionResult<IReadOnlyList<ProfesorFaltaDto>>> GetFaltasPorCurso([FromRoute] int idCurso, CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var profesorId))
        {
            return Unauthorized();
        }

        var faltas = await _profesorFaltasService.GetFaltasPorCursoAsync(profesorId, idCurso, cancellationToken);
        if (faltas is null)
        {
            return Forbid();
        }

        return Ok(faltas);
    }

    [HttpPost("faltas")]
    public async Task<ActionResult<ProfesorFaltaDto>> CreateFalta([FromBody] ProfesorFaltaRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var profesorId))
        {
            return Unauthorized();
        }

        var faltaCreada = await _profesorFaltasService.CrearFaltaAsync(profesorId, request, cancellationToken);
        if (faltaCreada is null)
        {
            return BadRequest(new { error = "No se pudo crear la falta", details = "Revisa el alumno, curso, estado o tipo de falta." });
        }

        return CreatedAtAction(nameof(GetFaltasPorCurso), new { idCurso = faltaCreada.IdCurso }, faltaCreada);
    }

    [HttpGet("cursos")]
    public async Task<ActionResult<IReadOnlyList<ProfesorCursoDto>>> GetMisCursos(CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var profesorId))
        {
            return Unauthorized();
        }

        var cursos = await _profesorFaltasService.GetMisCursosAsync(profesorId, cancellationToken);
        return Ok(cursos);
    }

    [HttpGet("cursos/{idCurso:int}/alumnos")]
    public async Task<ActionResult<IReadOnlyList<ProfesorAlumnoDto>>> GetAlumnosDeCurso([FromRoute] int idCurso, CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var profesorId))
        {
            return Unauthorized();
        }

        var alumnos = await _profesorFaltasService.GetAlumnosDeCursoAsync(profesorId, idCurso, cancellationToken);
        if (alumnos is null)
        {
            return Forbid();
        }

        return Ok(alumnos);
    }

    [HttpDelete("faltas/{id:int}")]
    public async Task<IActionResult> EliminarFalta([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var profesorId))
        {
            return Unauthorized();
        }

        var resultado = await _profesorFaltasService.EliminarFaltaAsync(profesorId, id, cancellationToken);

        return resultado switch
        {
            null => Forbid(),
            false => NotFound(),
            true => NoContent()
        };
    }

    private bool TryGetUsuarioId(out int usuarioId)
    {
        usuarioId = 0;
        var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(usuarioIdClaim, out usuarioId);
    }
}