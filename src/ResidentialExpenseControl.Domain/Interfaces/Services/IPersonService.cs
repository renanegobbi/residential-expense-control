using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface IPersonService
    {
        Task<IOutput> GetAll(SearchPeopleInput input);
        Task<PersonOutput> GetPersonById(int personId);
        Task<IOutput> GetPerson(int personId);
        Task<IOutput> Create(Person person);
        Task<IOutput> Update(Person person);
        Task<IOutput> Delete(int id);
    }
}
