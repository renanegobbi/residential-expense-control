using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class CategoryOutput
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public CategoryPurpose Purpose { get; set; }
    }
}
