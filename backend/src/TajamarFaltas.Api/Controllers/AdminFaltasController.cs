using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Api.Authorization;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.FaltaManagement.Models;

namespace TajamarFaltas.Api.Controllers
{
    [ApiController]
    [Route("api/admin/faltas")]
    [Authorize(Policy = PolicyNames.AdministradorOnly)]
    public class AdminFaltasController : ControllerBase
    {
        private readonly IAdminFaltasService _service;

        public AdminFaltasController(IAdminFaltasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var items = await _service.GetAllFaltasAsync(cancellationToken);
            return Ok(items);
        }

        [HttpPatch("{id}/justificacion")]
        public async Task<IActionResult> UpdateJustificacion(int id, [FromBody] AdminFaltaJustificacionRequest request, CancellationToken cancellationToken)
        {
            var ok = await _service.UpdateJustificacionAsync(id, request.EsJustificada, cancellationToken);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarFalta(int id, CancellationToken cancellationToken)
        {
            var ok = await _service.EliminarFaltaAsync(id, cancellationToken);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
