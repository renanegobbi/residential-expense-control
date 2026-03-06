using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IOutput> GetAll(SearchCategoryInput input);
        Task<CategoryOutput> GetCategoryById(int categoryId);
        Task<IOutput> GetCategory(int categoryId);
        Task<IOutput> Create(Category category);
        Task<IOutput> Update(Category category);
        Task<IOutput> Delete(int id);
    }
}
