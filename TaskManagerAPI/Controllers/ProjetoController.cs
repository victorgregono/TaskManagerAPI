using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    [AllowAnonymous]
    [Route("api/projeto")]
    public class ProjetoController : ApiController
    {
        private readonly IProjetoService _service;

        public ProjetoController(IProjetoService service)
            => _service = service;

        /// <summary>
        /// Listagem de Projetos - listar todos os projetos do usuário.
        /// </summary>
        /// <returns>Uma lista de projetos do usuário.</returns>
        /// <response code="200">Retorna a lista de projetos do usuário.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjetoViewModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ProjetoViewModel>>> Get()
            => CustomResponse(await _service.FindAll());

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjetoViewModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProjetoViewModel>> GetById(int id)
            => CustomResponse(await _service.FindById(id));

        [HttpPost]
        [ProducesResponseType(typeof(ProjetoViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProjetoViewModel>> Create([FromBody] ProjetoViewModel projeto)
            => CustomResponse(await _service.Create(projeto));

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(int id)
            => CustomResponse(await _service.Delete(id));
    }
}
