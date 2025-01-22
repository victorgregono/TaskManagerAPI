using FluentValidation;

namespace TaskManager.Domain.Models.Validations
{
    public class ProjetoValidator : AbstractValidator<Projeto>
    {
        public ProjetoValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .NotNull().WithMessage("O nome não pode ser nulo.")
                .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

            RuleFor(e => e.UsuarioId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
                .NotNull().WithMessage("O ID do usuário não pode ser nulo.");
        }
    }
}
