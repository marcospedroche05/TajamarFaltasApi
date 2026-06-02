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

    private bool TryGetUsuarioId(out int usuarioId)
    {
        usuarioId = 0;
        var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(usuarioIdClaim, out usuarioId);
    }
}