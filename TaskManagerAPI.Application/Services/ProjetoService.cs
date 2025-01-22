using AutoMapper;
using System.Net;
using TaskManager.Application.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using TaskManager.Domain.Models.Validations;

namespace TaskManager.Application.Services
{
    public class ProjetoService : Service, IProjetoService
    {
        private readonly IProjetoRepository _repository;
        private readonly ITarefaRepository _tarefaRepository;

        public ProjetoService(IMapper mapper, IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository) : base(mapper)
        {
            _repository = projetoRepository;
            _tarefaRepository = tarefaRepository;
        }

        public async Task<OperationResult> Create(ProjetoViewModel projeto)
        {
            // Verificar se o projeto já existe
            var existingProjeto = await _repository.GetByNameAsync(projeto.Nome);
            if (existingProjeto != null)
                return Error(ErrorMessages.ProjectNameAlreadyExistsError( projeto.Nome), HttpStatusCode.BadRequest);

            var entity = Mapper.Map<Projeto>(projeto);

            // Validar a entidade Projeto
            if (!EntityIsValid(new ProjetoValidator(), entity))
                return Error();
            
            await _repository.CreateAsync(entity);
            return Success();
        }

        

        public async Task<OperationResult> FindAll()
        {
            var projetos = await _repository.GetProjetosDetalhadosAsync();
            return Success(projetos);
        }

        public async Task<OperationResult> FindById(int id)
        {
            var projeto = await _repository.GetByIdAsync(id);
            if (projeto == null)
                return Error("Projeto não encontrado.", HttpStatusCode.NotFound);

            return Success(projeto);
        }

        public async Task<OperationResult> Update(int id, ProjetoViewModel projeto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return Error("Projeto não encontrado.", HttpStatusCode.NotFound);

            entity.Nome = projeto.Nome;
            entity.UsuarioId = projeto.UsuarioId;

            await _repository.UpdateAsync(entity);
            return Success();
        }

        public async Task<OperationResult> Delete(int id)
        {
            var tarefasPendentes = await _tarefaRepository.GetPendentesByProjetoIdAsync(id);
            if (tarefasPendentes.Any())
                return Error("Não é possível remover o projeto enquanto houver tarefas pendentes.", HttpStatusCode.BadRequest);

            var projeto = await _repository.GetByIdAsync(id);
            if (projeto == null)
                return Error("Projeto não encontrado.", HttpStatusCode.NotFound);

            await _repository.DeleteAsync(id);
            return Success();
        }
    }
}

