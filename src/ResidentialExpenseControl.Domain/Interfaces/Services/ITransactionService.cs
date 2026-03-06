using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<IOutput> GetAll(SearchTransactionInput input);
        Task<TransactionOutput> GetTransactionById(int transactionId);
        Task<IOutput> GetTransaction(int transactionId);
        Task<IOutput> Create(Transaction transaction);
        Task<IOutput> Update(Transaction transaction);
        Task<IOutput> Delete(int id);

    }
}
