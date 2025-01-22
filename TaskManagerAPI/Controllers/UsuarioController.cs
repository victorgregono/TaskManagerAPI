using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    [AllowAnonymous]
    [Route("api/usuario")]
    public class UsuarioController : ApiController
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        => _service = service;

        [HttpGet]
        //[Route("teste")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioViewModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> Get()
            => CustomResponse(await _service.FindAll());

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioViewModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UsuarioViewModel>> GetById(int id)
            => CustomResponse(await _service.FindById(id));

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UsuarioViewModel>> Create([FromBody] UsuarioViewModel usuario)
            => CustomResponse(await _service.Create(usuario));

       
    }
}
