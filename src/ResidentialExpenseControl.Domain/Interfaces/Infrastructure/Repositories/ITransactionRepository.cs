using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>
    /// Repository contract for Transaction table.
    /// </summary>
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Tuple<Transaction[], double>> GetAll(SearchTransactionInput input);
        Task<bool> ExistsByCategoryId(int categoryId);
        Task<bool> HasIncomeByPersonId(int personId);
    }
}
