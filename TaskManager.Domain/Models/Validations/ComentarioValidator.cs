using FluentValidation;
using System;

namespace TaskManager.Domain.Models.Validations
{
    public class ComentarioValidator : AbstractValidator<Comentario>
    {
        public ComentarioValidator()
        {
            RuleFor(e => e.Texto)
                .NotEmpty().WithMessage("O texto é obrigatório.")
                .MaximumLength(500).WithMessage("O texto deve ter no máximo 500 caracteres.");

            RuleFor(e => e.DataComentario)
                .NotEmpty().WithMessage("A data do comentário é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data do comentário não pode ser no futuro.");

            RuleFor(e => e.TarefaId)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório.");

            RuleFor(e => e.UsuarioId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
        }
    }
}



