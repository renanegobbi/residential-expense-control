using ResidentialExpenseControl.Domain.Entities;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task Create(Transaction transaction);
    }
}
