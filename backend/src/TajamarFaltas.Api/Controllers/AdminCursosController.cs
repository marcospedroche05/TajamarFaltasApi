using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Api.Authorization;
using TajamarFaltas.Application.FaltaManagement;

namespace TajamarFaltas.Api.Controllers
{
    [ApiController]
    [Route("api/admin/cursos")]
    [Authorize(Policy = PolicyNames.AdministradorOnly)]
    public class AdminCursosController : ControllerBase
    {
        private readonly IAdminCursosService _service;

        public AdminCursosController(IAdminCursosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var items = await _service.GetAllCursosAsync(cancellationToken);
            return Ok(items);
        }

        [HttpGet("{idCurso:int}/alumnos")]
        public async Task<IActionResult> GetAlumnos(int idCurso, CancellationToken cancellationToken)
        {
            var items = await _service.GetAlumnosDeCursoAsync(idCurso, cancellationToken);
            return Ok(items);
        }

        [HttpGet("{idCurso:int}/faltas")]
        public async Task<IActionResult> GetFaltas(int idCurso, CancellationToken cancellationToken)
        {
            var items = await _service.GetFaltasDeCursoAsync(idCurso, cancellationToken);
            return Ok(items);
        }
    }
}
