using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Api.ViewModels.Category
{
    public class UpdateCategoryViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public CategoryPurpose CategoryPurpose { get; set; }
    }
}
