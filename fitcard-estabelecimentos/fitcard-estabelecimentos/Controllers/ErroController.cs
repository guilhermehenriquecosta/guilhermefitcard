using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fitcard_estabelecimentos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ErroController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}