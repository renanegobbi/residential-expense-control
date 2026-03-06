using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Api.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        public string Description { get; set; }
        public CategoryPurpose CategoryPurpose { get; set; }
    }
}
