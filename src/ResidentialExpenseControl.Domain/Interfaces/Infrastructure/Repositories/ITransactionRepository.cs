using ResidentialExpenseControl.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>Repository contract for Transaction table.</summary>
    public interface ITransactionRepository : IRepository<Transaction>
    {
        // Useful to show lists with includes (Person/Category) without leaking EF outside
        Task<List<Transaction>> GetAllWithDetails();

        Task<List<Transaction>> GetByPersonId(int personId);
        Task<List<Transaction>> GetByCategoryId(int categoryId);
    }
}
