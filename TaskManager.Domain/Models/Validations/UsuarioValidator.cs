using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Models.Validations
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .NotNull().WithMessage("O nome não pode ser nulo.")
                .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

            RuleFor(e => e.Funcao)
                .NotEmpty().WithMessage("A função é obrigatória.")
                .NotNull().WithMessage("A função não pode ser nula.")
                .MaximumLength(20).WithMessage("A função deve ter no máximo 20 caracteres.");
        }
    }
}

