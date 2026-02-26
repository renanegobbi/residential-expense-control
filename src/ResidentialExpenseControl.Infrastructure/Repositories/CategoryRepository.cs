using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }
    }
}
