using CalculoIRRFeINSS.Domain.Entidades;
using FluentValidation;

namespace CalculoIRRFeINSS.Domain.Validators
{
    public class EmpregadoValidator : AbstractValidator<Empregado>
    {
        public EmpregadoValidator()
        {
            RuleFor(e => e.Nome)
               .NotEmpty().WithMessage("O nome do empregado é obrigatório.")
               .MinimumLength(1).WithMessage("O nome deve ter pelo menos 1 caractere.")
               .MaximumLength(40).WithMessage("O nome deve ter no máximo 40 caracteres.");

            RuleFor(e => e.CPF)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Length(11).WithMessage("O CPF deve conter exatamente 11 dígitos.")
                .Matches(@"^\d{11}$").WithMessage("O CPF deve conter apenas números.");

            RuleFor(e => e.Dependentes)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade de dependentes deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(99).WithMessage("A quantidade de dependentes não pode ultrapassar 2 dígitos.");

            RuleFor(e => e.Rubricas)
                .NotNull().WithMessage("A lista de rubricas não pode ser nula.");
        }
    }
}
