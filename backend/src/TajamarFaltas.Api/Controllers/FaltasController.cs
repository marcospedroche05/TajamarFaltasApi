using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;
using TajamarFaltas.Api.Authorization;

namespace TajamarFaltas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyNames.AlumnoOnly)]
public class FaltasController : ControllerBase
{
    private readonly IMisFaltasQueryService _misFaltasQueryService;

    public FaltasController(IMisFaltasQueryService misFaltasQueryService)
    {
        _misFaltasQueryService = misFaltasQueryService;
    }

    [HttpGet("mis-faltas")]
    public async Task<ActionResult<IReadOnlyList<MisFaltaDto>>> GetMisFaltas(CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var usuarioId))
            return Unauthorized();

        var faltas = await _misFaltasQueryService.GetMisFaltasAsync(usuarioId, cancellationToken);
        return Ok(faltas);
    }

    [HttpGet("mis-cursos")]
    public async Task<ActionResult<IReadOnlyList<AlumnoCursoDto>>> GetMisCursos(CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var usuarioId))
            return Unauthorized();

        var cursos = await _misFaltasQueryService.GetMisCursosAsync(usuarioId, cancellationToken);
        return Ok(cursos);
    }

    [HttpGet("resumen-asistencia")]
    public async Task<ActionResult<IReadOnlyList<ResumenAsistenciaDto>>> GetResumenAsistencia(CancellationToken cancellationToken)
    {
        if (!TryGetUsuarioId(out var usuarioId))
            return Unauthorized();

        var resumen = await _misFaltasQueryService.GetResumenAsistenciaAsync(usuarioId, cancellationToken);
        return Ok(resumen);
    }

    private bool TryGetUsuarioId(out int usuarioId)
    {
        usuarioId = 0;
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claim, out usuarioId);
    }
}