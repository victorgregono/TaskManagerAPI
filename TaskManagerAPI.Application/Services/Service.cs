using AutoMapper;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Services
{
    /// <summary>
    /// Classe base abstrata para serviços de aplicação, fornecendo métodos utilitários para validação e manipulação de resultados de operações.
    /// </summary>
    public abstract class Service
    {
        protected readonly IMapper Mapper;

        /// <summary>
        /// Construtor da classe Service.
        /// </summary>
        /// <param name="mapper">Instância do AutoMapper para mapeamento de objetos.</param>
        protected Service(IMapper mapper) => Mapper = mapper;

        private ValidationResult _validationResult;

        /// <summary>
        /// Retorna um resultado de operação com erro.
        /// </summary>
        /// <returns>Resultado de operação com erro.</returns>
        protected OperationResult Error() => new OperationResult(_validationResult);

        /// <summary>
        /// Retorna um resultado de operação com erro e código de status HTTP.
        /// </summary>
        /// <param name="statusCode">Código de status HTTP.</param>
        /// <returns>Resultado de operação com erro e código de status HTTP.</returns>
        protected OperationResult Error(HttpStatusCode statusCode) => new OperationResult(_validationResult, statusCode);

        /// <summary>
        /// Retorna um resultado de operação com erro e mensagem de erro.
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro.</param>
        /// <param name="statusCode">Código de status HTTP (padrão: BadRequest).</param>
        /// <returns>Resultado de operação com erro e mensagem de erro.</returns>
        protected OperationResult Error(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, errorMessage) };
            return new OperationResult(new ValidationResult(failures), statusCode);
        }

        /// <summary>
        /// Retorna um resultado de operação bem-sucedida com um objeto opcional.
        /// </summary>
        /// <param name="obj">Objeto opcional a ser retornado.</param>
        /// <returns>Resultado de operação bem-sucedida.</returns>
        protected OperationResult Success(object obj = null) => new OperationResult(obj);

        /// <summary>
        /// Retorna um resultado de operação bem-sucedida com um ID.
        /// </summary>
        /// <param name="id">ID do objeto criado.</param>
        /// <returns>Resultado de operação bem-sucedida.</returns>
        protected OperationResult Success(long id) => new OperationResult(new PostViewModel(id));

        /// <summary>
        /// Notifica um erro de validação.
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro.</param>
        protected void NotifyError(string errorMessage)
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, errorMessage) };
            _validationResult = new ValidationResult(failures);
        }

        /// <summary>
        /// Valida uma entidade usando um validador especificado.
        /// </summary>
        /// <typeparam name="TV">Tipo do validador.</typeparam>
        /// <typeparam name="TE">Tipo da entidade.</typeparam>
        /// <param name="validator">Instância do validador.</param>
        /// <param name="entity">Entidade a ser validada.</param>
        /// <returns>True se a entidade for válida, caso contrário, false.</returns>
        protected bool EntityIsValid<TV, TE>(TV validator, TE entity) where TV : AbstractValidator<TE> where TE : BaseEntity
        {
            var result = validator.Validate(entity);
            if (result.IsValid) return true;

            _validationResult = result;
            return false;
        }

        /// <summary>
        /// Valida uma lista de entidades usando um validador especificado.
        /// </summary>
        /// <typeparam name="TV">Tipo do validador.</typeparam>
        /// <typeparam name="TE">Tipo da entidade.</typeparam>
        /// <param name="validator">Instância do validador.</param>
        /// <param name="entities">Lista de entidades a serem validadas.</param>
        /// <returns>True se todas as entidades forem válidas, caso contrário, false.</returns>
        protected bool EntityIsValid<TV, TE>(TV validator, IEnumerable<TE> entities) where TV : AbstractValidator<TE> where TE : BaseEntity
        {
            foreach (var item in entities)
            {
                var result = validator.Validate(item);
                if (!result.IsValid)
                {
                    _validationResult = result;
                    return false;
                }
            }

            return true;
        }
    }
}



