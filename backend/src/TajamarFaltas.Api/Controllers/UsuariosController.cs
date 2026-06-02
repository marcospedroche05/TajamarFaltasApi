using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Api.Authorization;
using TajamarFaltas.Application.Usuarios;
using TajamarFaltas.Application.Usuarios.Models;

namespace TajamarFaltas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyNames.ProfesorOrAdministrador)]
public class UsuariosController : ControllerBase
{
    private readonly IUsuariosQueryService _usuariosQueryService;

    public UsuariosController(IUsuariosQueryService usuariosQueryService)
    {
        _usuariosQueryService = usuariosQueryService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var usuario = await _usuariosQueryService.GetByIdAsync(id, cancellationToken);
        if (usuario is null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }
}
