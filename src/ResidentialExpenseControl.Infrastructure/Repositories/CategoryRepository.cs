using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }

        public async Task<Tuple<Category[], double>> GetAll(SearchCategoryInput input)
        {
            IEnumerable<Category> records = Db.Categories.AsNoTracking();

            // Filters
            if (input.Id.HasValue)
                records = records.Where(p => p.Id == input.Id.Value);

            if (!string.IsNullOrWhiteSpace(input.Description))
                records = records.Where(p => p.Description.Contains(input.Description));

            if (input.Purpose.HasValue)
                records = records.Where(c => c.Purpose == input.Purpose.Value);

            switch (input.OrderBy)
            {
                case CategoryOrderBy.Description:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(c => c.Description)
                        : records.OrderBy(c => c.Description);
                    break;
                case CategoryOrderBy.Purpose:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(c => c.Purpose)
                        : records.OrderBy(c => c.Purpose);
                    break;
                default:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(c => c.Id)
                        : records.OrderBy(c => c.Id);
                    break;
            }

            var totalRecords = Convert.ToDouble(records.Count());

            records = records
                .Skip((int)input.PageSize * ((int)input.PageIndex - 1))
                .Take((int)input.PageSize)
                .ToList();

            if (input.HasPagination())
            {
                return new Tuple<Category[], double>(records.ToArray(), totalRecords);
            }
            else
            {
                return new Tuple<Category[], double>(records.ToArray(), totalRecords);
            }
        }

        public async Task<bool> ExistsByDescription(string description)
        {
            var normalized = description?.Trim().ToLower();
            return await Db.Categories.AsNoTracking()
                .AnyAsync(c => c.Description.Trim().ToLower() == normalized);
        }

        public async Task<bool> ExistsOtherWithDescription(string description, int ignoreId)
        {
            var normalized = (description ?? "").Trim().ToLower();

            return await Db.Categories.AsNoTracking()
                .AnyAsync(c => c.Id != ignoreId &&
                               c.Description.Trim().ToLower() == normalized);
        }
    }
}
