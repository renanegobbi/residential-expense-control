using FluentValidation;

namespace ResidentialExpenseControl.Domain.Entities.Validations
{
    public class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .WithName("Nome")
                .MaximumLength(200).WithMessage("O campo {PropertyName} deve ter no máximo {MaxLength} caracteres.");

            RuleFor(p => p.Age)
                .GreaterThanOrEqualTo(0)
                .WithName("Idade")
                .WithMessage("O campo {PropertyName} deve ser um número não negativo.");

            RuleFor(x => x.Age)
                .InclusiveBetween(0, 130)
                .WithName("Idade")
                .WithMessage("O campo {PropertyName} deve estar entre {From} e {To} anos.");
        }
    }
}
