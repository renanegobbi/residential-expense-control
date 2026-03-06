using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Domain.Extensions
{
    public static class CategoryMappingExtensions
    {
        public static CategoryOutput ToCategoryOutput(this Category entity)
        {
            if (entity == null) return null;

            return new CategoryOutput
            {
                Id = entity.Id,
                Description = entity.Description,
                Purpose = entity.Purpose
            };
        }

        public static Category ToCategory(this CategoryOutput model)
        {
            if (model == null) return null;

            return new Category
            {
                Id = model.Id,
                Description = model.Description,
                Purpose = model.Purpose
            };
        }
    }
}
