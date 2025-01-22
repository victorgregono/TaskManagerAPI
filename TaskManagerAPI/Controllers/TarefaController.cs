using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    [AllowAnonymous]
    [Route("api/tarefa")]
    public class TarefaController : ApiController
    {
        private readonly ITarefaService _service;

        public TarefaController(ITarefaService service)
            => _service = service;

        /// <summary>
        /// Visualização de Tarefas - visualizar todas as tarefas de um projeto específico.
        /// </summary>
        /// <param name="projetoId">ID do projeto.</param>
        /// <returns>Uma lista de tarefas do projeto especificado.</returns>
        /// <response code="200">Retorna a lista de tarefas do projeto.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("projeto/{projetoId}")]
        [ProducesResponseType(typeof(IEnumerable<TarefaViewModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TarefaViewModel>>> GetByProjetoId(int projetoId)
            => CustomResponse(await _service.FindByProjetoId(projetoId));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaViewModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TarefaViewModel>> GetById(int id)
            => CustomResponse(await _service.FindById(id));
        /// <summary>
        /// Gera relatórios de desempenho, como o número médio de tarefas concluídas
        /// por usuário nos últimos 30 dias.
        /// </summary>
        /// <returns>Relatório de desempenho dos usuários.</returns>
        /// <response code="200">Retorna o relatório de desempenho.</response>
        /// <response code="403">Acesso negado. Apenas usuários com a função de "gerente" podem acessar este endpoint.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("relatorio-desempenho")]
        [Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetPerformanceReport()
        
            => CustomResponse(await _service.GetPerformanceReport());
        
        [HttpPost]
        [ProducesResponseType(typeof(TarefaViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TarefaViewModel>> Create([FromBody] TarefaViewModel tarefa)
            => CustomResponse(await _service.Create(tarefa));

        /// <summary>
        /// Atualizar uma Tarefa existente.
        /// </summary>
        /// <param name="id">ID da tarefa.</param>
        /// <param name="tarefa">Dados atualizados da tarefa.</param>
        /// <returns>Resultado da operação.</returns>
        /// <response code="200">Tarefa atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Tarefa não encontrada.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Update(int id, [FromBody] TarefaViewModel tarefa)
            => CustomResponse(await _service.Update(id, tarefa));

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(int id)
            => CustomResponse(await _service.Delete(id));
    }
}
