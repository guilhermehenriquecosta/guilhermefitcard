using fitcard_estabelecimentos.data.Repository;
using fitcard_estabelecimentos.domain.Domain;
using fitcard_estabelecimentos.domain.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace fitcard_estabelecimentos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EstabelecimentoController: ControllerBase
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;

        public EstabelecimentoController(IEstabelecimentoRepository estabelecimentoRepository)
        {
            _estabelecimentoRepository = estabelecimentoRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> BuscaTodos()
        {
            var estabelecimentos = await _estabelecimentoRepository.BuscaTodos();
            if (!estabelecimentos.Any())
                return NoContent();

            return Ok(estabelecimentos);
        }

        [HttpGet]
        [Route("{id}", Name = "BuscaUm")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscaUm([FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            var categoria = await _estabelecimentoRepository.BuscaUm(id);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Insere([FromBody]Estabelecimento estabelecimento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                //return BadRequest(new ApiBadRequestResponse(ModelState));

            var id = await _estabelecimentoRepository.Insere(estabelecimento);

            estabelecimento.Id = id;

            return CreatedAtRoute(nameof(BuscaUm), new { id = id }, estabelecimento);
        }

        [HttpPut]
        [Route("{id}", Name = "AtualizaEstabelecimento")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualiza([FromRoute]Guid id, [FromBody]Estabelecimento estabelecimento)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                //return BadRequest(new ApiBadRequestResponse(ModelState));

            var qtdlinhasAfetadas = await _estabelecimentoRepository.Edita(estabelecimento);
            if (qtdlinhasAfetadas != 1)
                return NotFound();

            return Accepted();
        }

        [HttpDelete]
        [Route("{id}", Name = "RemoveEstabelecimento")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            var qtdlinhasAfetadas = await _estabelecimentoRepository.Remove(id);
            if (qtdlinhasAfetadas != 1)
                return NotFound();

            return Ok();
        }
    }
}
