using CalculoIRRFeINSS.Domain.Entidades;
using FluentValidation;

namespace CalculoIRRFeINSS.Domain.Validators
{
    public class RubricaValidator : AbstractValidator<Rubrica>
    {
        public RubricaValidator()
        {
            RuleFor(r => r.Codigo)
                .GreaterThan(0).WithMessage("O código da rubrica deve ser maior que zero.")
                .LessThanOrEqualTo(9999).WithMessage("O código da rubrica deve ter no máximo 4 dígitos.");

            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("A descrição da rubrica é obrigatória.")
                .MaximumLength(40).WithMessage("A descrição da rubrica deve ter no máximo 40 caracteres.");

            RuleFor(r => r.TipoRubrica)
                .IsInEnum().WithMessage("O tipo da rubrica deve ser P (Provento) ou D (Desconto).");

            RuleFor(r => r.Valor)
                .GreaterThan(0).WithMessage("O valor da rubrica deve ser maior que zero.")
                .LessThanOrEqualTo(999999999999.99m).WithMessage("O valor da rubrica ultrapassa o limite permitido.");
        }
    }
}
