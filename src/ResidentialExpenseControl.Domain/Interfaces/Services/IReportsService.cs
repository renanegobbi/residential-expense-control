using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface IReportsService
    {
        Task<IOutput> GetTotalsByPerson();
        Task<IOutput> GetTotalsByCategory();
    }
}
