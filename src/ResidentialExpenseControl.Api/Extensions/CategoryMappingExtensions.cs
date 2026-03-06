using ResidentialExpenseControl.Api.ViewModels.Category;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Api.Extensions
{
    public static class CategoryMappingExtensions
    {
        public static SearchCategoryInput ToSearchCategoryInput(this SearchCategoryViewModel model)
        {
            if (model == null) return null;

            var input = new SearchCategoryInput(
                model.OrderBy,
                model.OrderDirection ?? "ASC",
                model.PageIndex,
                model.PageSize
            )
            {
                Id = model.Id,
                Description = model.Description
            };

            return input;
        }

        public static Category ToCategory(this CreateCategoryViewModel model)
        {
            if (model == null) return null;

            return new Category
            {
                Description = model.Description,
                Purpose = model.CategoryPurpose
            };
        }

        public static Category ToCategory(this UpdateCategoryViewModel model)
        {
            if (model == null) return null;

            return new Category
            {
                Id = model.Id,
                Description = model.Description,
                Purpose = model.CategoryPurpose
            };
        }
    }
}
