using FluentValidation;

namespace ResidentialExpenseControl.Domain.Entities.Validations
{
    public class CategoryValidation : AbstractValidator<Category>
    {
        public CategoryValidation()
        {
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .MaximumLength(400)
                .WithName("Descrição")
                .WithMessage("O campo {PropertyName} deve ter no máximo {MaxLength} caracteres.");

            RuleFor(c => c.Purpose)
                .IsInEnum()
                .WithName("Finalidade")
                .WithMessage("O campo {PropertyName} possui um valor inválido. Valores permitidos: Despesa, Receita ou Ambas.");
        }
    }
}
