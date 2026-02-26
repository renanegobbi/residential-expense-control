using FluentValidation;

namespace ResidentialExpenseControl.Domain.Entities.Validations
{
    public class TransactionValidation : AbstractValidator<Transaction>
    {
        public TransactionValidation()
        {
            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .MaximumLength(400).WithMessage("O campo {PropertyName} deve ter no máximo {MaxLength} caracteres.");

            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .WithMessage("O campo {PropertyName} deve ser um valor positivo.");

            RuleFor(t => t.Type)
                .IsInEnum()
                .WithMessage("O campo {PropertyName} possui um valor inválido.");

            RuleFor(t => t.CategoryId)
                .GreaterThan(0)
                .WithMessage("O campo {PropertyName} deve ser maior que zero.");

            RuleFor(t => t.PersonId)
                .GreaterThan(0)
                .WithMessage("O campo {PropertyName} deve ser maior que zero.");
        }
    }
}
