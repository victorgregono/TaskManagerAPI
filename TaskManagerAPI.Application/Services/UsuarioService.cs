using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using TaskManager.Domain.Models.Validations;

namespace TaskManager.Application.Services
{
    public class UsuarioService : Service, IUsuarioService
    {
        public readonly IUsuarioRepository _repository;
        public UsuarioService(IMapper mapper, IUsuarioRepository usuarioRepository) : base(mapper) 
            => _repository = usuarioRepository;

        public async Task<OperationResult> Create(UsuarioViewModel usuario)
        {
            var entities = Mapper.Map<Usuario>(usuario);
            if (!EntityIsValid(new UsuarioValidator(), entities))
                return Error();

            await _repository.InsertAsync(entities);
            return Success();
        }
        

        public async Task<OperationResult> FindAll()
        {
            var usuarios = await _repository.GetAllAsync();
            if(!usuarios.Any())
                return Error(ErrorMessages.UserNotFoundError(typeof(UsuarioViewModel)), HttpStatusCode.NotFound);

            return Success(usuarios);
        }

        public async Task<OperationResult> FindById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null)
                return Error(ErrorMessages.IdNotFoundError(typeof(UsuarioViewModel), id), HttpStatusCode.NotFound);

            return Success(entity);
        }

       
    }
}
