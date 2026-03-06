using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface ITotalsService
    {
        Task<IOutput> GetTotalsByPeople(TotalsFilterByPersonInput input);
        Task<IOutput> GetTotalsByCategory(TotalsFilterByCategoryInput input);
    }
}
