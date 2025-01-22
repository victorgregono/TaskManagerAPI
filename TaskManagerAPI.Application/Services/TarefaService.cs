using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using TaskManager.Application.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Data.Repositories;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using TaskManager.Domain.Models.Validations;

namespace TaskManager.Application.Services
{
    public class TarefaService : Service, ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IHistoricoTarefaRepository _historicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProjetoRepository _projetoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TarefaService(IMapper mapper, ITarefaRepository tarefaRepository,
            IHistoricoTarefaRepository historicoRepository,
            IUsuarioRepository usuarioRepository,
            IProjetoRepository projetoRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper)
        {
            _repository = tarefaRepository;
            _historicoRepository = historicoRepository;
            _usuarioRepository = usuarioRepository;
            _projetoRepository = projetoRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<OperationResult> Create(TarefaViewModel tarefa)
        {
            // Verificar se o projeto existe
            var projeto = await _projetoRepository.GetByIdAsync(tarefa.ProjetoId);
            if (projeto is null)
                return Error(ErrorMessages.ProjectNotFoundError(typeof(TarefaViewModel), tarefa.ProjetoId), HttpStatusCode.NotFound);

            // Verificar o limite de tarefas por projeto
            if (await _repository.CountByProjetoIdAsync(tarefa.ProjetoId) >= 20)
                return Error(ErrorMessages.ProjectTaskLimitReached(), HttpStatusCode.BadRequest);

            var entity = Mapper.Map<Tarefa>(tarefa);

            // Validar a entidade Tarefa
            if (!EntityIsValid(new TarefaValidator(), entity))
                return Error();

            await _repository.InsertAsync(entity);

            // Registrar o histórico de criação da tarefa
            var historico = new HistoricoTarefa
            {
                TarefaId = entity.Id,
                CampoModificado = "Criação",
                ValorNovo = "Tarefa criada",
                DataModificacao = DateTime.UtcNow,
                UsuarioId = projeto.UsuarioId 
            };
            await _historicoRepository.InsertAsync(historico);

            return Success();
        }


        public async Task<OperationResult> Update(int id, TarefaViewModel tarefa)
        {
            var existingTarefa = await _repository.GetByIdAsync(id);
            if (existingTarefa == null)
                return Error("Tarefa não encontrada.", HttpStatusCode.NotFound);

            if ((PrioridadeTarefa)existingTarefa.Prioridade != tarefa.Prioridade)
                return Error("Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.", HttpStatusCode.BadRequest);

            var user = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var historico = new List<HistoricoTarefa>();
            AdicionarHistoricoSeNecessario(historico, existingTarefa, tarefa, userId);

            if (historico.Any())
            {
                await _historicoRepository.InsertAllAsync(historico);
                await _repository.UpdateAsync(existingTarefa);
            }

            return Success();
        }

        private void AdicionarHistoricoSeNecessario(List<HistoricoTarefa> historico, Tarefa existingTarefa, TarefaViewModel tarefa, int userId)
        {
            VerificarEAdicionarHistorico(historico, existingTarefa, tarefa, userId, "Status", existingTarefa.Status, (int)tarefa.Status, v => existingTarefa.Status = v);
            VerificarEAdicionarHistorico(historico, existingTarefa, tarefa, userId, "Titulo", existingTarefa.Titulo, tarefa.Titulo, v => existingTarefa.Titulo = v);
            VerificarEAdicionarHistorico(historico, existingTarefa, tarefa, userId, "Descricao", existingTarefa.Descricao, tarefa.Descricao, v => existingTarefa.Descricao = v);
            VerificarEAdicionarHistorico(historico, existingTarefa, tarefa, userId, "DataVencimento", existingTarefa.DataVencimento, tarefa.DataVencimento, v => existingTarefa.DataVencimento = v);
            VerificarEAdicionarHistorico(historico, existingTarefa, tarefa, userId, "Prioridade", existingTarefa.Prioridade, (int)tarefa.Prioridade, v => existingTarefa.Prioridade = v);
        }

        private void VerificarEAdicionarHistorico<T>(List<HistoricoTarefa> historico, Tarefa existingTarefa, TarefaViewModel tarefa, int userId, string campo, T valorAntigo, T valorNovo, Action<T> atualizarValor)
        {
            if (!EqualityComparer<T>.Default.Equals(valorAntigo, valorNovo))
            {
                historico.Add(new HistoricoTarefa
                {
                    TarefaId = existingTarefa.Id,
                    UsuarioId = userId,
                    DataModificacao = DateTime.UtcNow,
                    CampoModificado = campo,
                    ValorAntigo = valorAntigo?.ToString(),
                    ValorNovo = valorNovo?.ToString()
                });
                atualizarValor(valorNovo);
            }
        }




        public async Task<OperationResult> Delete(int id)
        {
            var tarefa = await _repository.GetByIdAsync(id);
            if (tarefa == null)
                return Error("Tarefa não encontrada.", HttpStatusCode.NotFound);

            await _repository.DeleteAsync(id);
            return Success();
        }

        public async Task<OperationResult> FindAll()
        {
            var tarefas = await _repository.GetAllAsync();
            return Success(tarefas);
        }

        public async Task<OperationResult> FindById(int id)
        {
            var tarefa = await _repository.GetByIdAsync(id);
            if (tarefa == null)
                return Error(ErrorMessages.TaskNotFoundError(typeof(Tarefa),id), HttpStatusCode.NotFound);

            return Success(tarefa);
        }

        public async Task<OperationResult> FindByProjetoId(int projetoId)
        {

            var tarefas = await _repository.GetByIdAsync(projetoId);
            return Success(tarefas);
        }


        public async Task<OperationResult> GetPerformanceReport()
        {

            var user = _httpContextAccessor.HttpContext.User;
            if (!user.IsInRole("Admin"))
            {
                return Error("Acesso negado. Apenas administradores podem acessar este recurso.", HttpStatusCode.BadRequest);


                //return Error("Authorization", "Acesso negado. Apenas administradores podem acessar este recurso.", HttpStatusCode.Forbidden);

            }


            var desempenhos = await _repository.GetPerformanceReport();

            if (!desempenhos.Any())
                return Error(ErrorMessages.DesempenhoNaoEncontrado(), HttpStatusCode.NotFound);

            return Success(desempenhos);
            
        }









        //public async Task<OperationResult> Update(int id, TarefaViewModel tarefa)
        //{
        //    var existingTarefa = await _repository.GetByIdAsync(id);
        //    if (existingTarefa == null)
        //        return Error("Tarefa não encontrada.", HttpStatusCode.NotFound);

        //    if ((PrioridadeTarefa)existingTarefa.Prioridade != tarefa.Prioridade)
        //        return Error("Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.", HttpStatusCode.BadRequest);

        //    var historico = new HistoricoTarefa
        //    {
        //        TarefaId = id,
        //        //UsuarioId = tarefa.UsuarioId,
        //        DataModificacao = DateTime.UtcNow,
        //        CampoModificado = "Status",
        //        ValorAntigo = existingTarefa.Status.ToString(),
        //        ValorNovo = tarefa.Status.ToString()
        //    };

        //    await _historicoRepository.CreateAsync(historico);

        //    existingTarefa.Status = (int)tarefa.Status;
        //    existingTarefa.Titulo = tarefa.Titulo;
        //    existingTarefa.Descricao = tarefa.Descricao;
        //    existingTarefa.DataVencimento = tarefa.DataVencimento;

        //    await _repository.UpdateAsync(existingTarefa);
        //    return Success();
        //}

    }
}

