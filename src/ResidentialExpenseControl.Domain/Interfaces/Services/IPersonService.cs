using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface IPersonService
    {
        Task Create(Person person);
        Task Update(Person person);
        Task Delete(int id);
    }
}
