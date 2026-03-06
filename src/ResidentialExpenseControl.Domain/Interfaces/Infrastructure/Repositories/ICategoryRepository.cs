using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>
    /// Repository contract for Category table.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Tuple<Category[], double>> GetAll(SearchCategoryInput input);

        Task<bool> ExistsByDescription(string description);

        Task<bool> ExistsOtherWithDescription(string description, int ignoreId);
    }
}
