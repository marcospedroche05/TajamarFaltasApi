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
        var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var faltas = await _misFaltasQueryService.GetMisFaltasAsync(usuarioId, cancellationToken);
        return Ok(faltas);
    }
}