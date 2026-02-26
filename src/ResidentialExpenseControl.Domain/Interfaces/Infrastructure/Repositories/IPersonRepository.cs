using ResidentialExpenseControl.Domain.Entities;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>Repository contract for Person table.</summary>
    public interface IPersonRepository : IRepository<Person>
    {
        Task<bool> ExistsByName(string name);
    }
}
