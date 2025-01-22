using FluentValidation;
using System;

namespace TaskManager.Domain.Models.Validations
{
    public class HistoricoTarefaValidator : AbstractValidator<HistoricoTarefa>
    {
        public HistoricoTarefaValidator()
        {
            RuleFor(e => e.CampoModificado)
                .NotEmpty().WithMessage("O campo modificado é obrigatório.")
                .MaximumLength(100).WithMessage("O campo modificado deve ter no máximo 100 caracteres.");

            RuleFor(e => e.DataModificacao)
                .NotEmpty().WithMessage("A data de modificação é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de modificação não pode ser no futuro.");

            RuleFor(e => e.TarefaId)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório.");

            RuleFor(e => e.UsuarioId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
        }
    }
}




