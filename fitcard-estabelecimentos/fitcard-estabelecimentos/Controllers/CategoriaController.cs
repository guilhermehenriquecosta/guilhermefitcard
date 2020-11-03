using System;
using System.Linq;
using System.Threading.Tasks;
using fitcard_estabelecimentos.data.Repository;
using fitcard_estabelecimentos.domain.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fitcard_estabelecimentos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> BuscaTodas()
        {
            var categorias = await _categoriaRepository.BuscaTodas();
            if (!categorias.Any())
                return NoContent();

            return Ok(categorias);
        }

        [HttpGet]
        [Route("{id}", Name = "BuscaUma")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscaUma([FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            var categoria = await _categoriaRepository.BuscaUma(id);

            if (categoria == null) 
                return NotFound();
            
            return Ok(categoria);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Insere([FromBody]Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var id = await _categoriaRepository.Insere(categoria);
            
            categoria.Id = id;
            
            return CreatedAtRoute(nameof(BuscaUma), new { id = id }, categoria);
        }

        [HttpPut]
        [Route("{id}", Name = "AtualizaCategoria")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualiza([FromRoute]Guid id, [FromBody]Categoria categoria)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var qtdlinhasAfetadas = await _categoriaRepository.Edita(categoria);
            if (qtdlinhasAfetadas != 1)
                return NotFound();

            return Accepted();
        }

        [HttpDelete]
        [Route("{id}", Name = "RemoveCategoria")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            var qtdlinhasAfetadas = await _categoriaRepository.Remove(id);
            if (qtdlinhasAfetadas != 1)
                return NotFound();

            return Ok();
        }
    }
}