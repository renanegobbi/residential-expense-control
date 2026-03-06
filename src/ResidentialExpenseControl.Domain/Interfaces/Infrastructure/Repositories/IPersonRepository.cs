using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>
    /// Repository contract for Person table.
    /// </summary>
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Tuple<Person[], double>> GetAll(SearchPeopleInput input);

        Task<bool> ExistsByName(string name);
    }
}
