using FluentValidation;
using System;

namespace TaskManager.Domain.Models.Validations
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        public TarefaValidator()
        {
            RuleFor(e => e.Titulo)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.");

            RuleFor(e => e.DataVencimento)
                .NotEmpty().WithMessage("A data de vencimento é obrigatória.")
                .GreaterThan(DateTime.Now).WithMessage("A data de vencimento deve ser no futuro.");

            //RuleFor(e => e.Status)
            //    .NotEmpty().WithMessage("O status é obrigatório.");

            //RuleFor(e => e.Prioridade)
            //    .NotEmpty().WithMessage("A prioridade é obrigatória.");

            RuleFor(e => e.ProjetoId)
                .NotEmpty().WithMessage("O ID do projeto é obrigatório.");
        }
    }
}


