using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IPersonRepository personRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier) : base(notifier)
        {
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Transaction transaction)
        {
            var person = await _personRepository.GetById(transaction.PersonId);
            if (person == null)
            {
                Notify("Pessoa não encontrada.");
                return;
            }

            if (person.Age < 18 && transaction.Type == TransactionType.Income)
            {
                Notify("Menores de 18 anos não podem registrar receitas.");
                return;
            }

            var category = await _categoryRepository.GetById(transaction.CategoryId);
            if (category == null)
            {
                Notify("Categoria não encontrada.");
                return;
            }

            if (transaction.Type == TransactionType.Expense &&
                category.Purpose == CategoryPurpose.Income)
            {
                Notify("Categoria inválida para despesa.");
                return;
            }

            if (transaction.Type == TransactionType.Income &&
                category.Purpose == CategoryPurpose.Expense)
            {
                Notify("Categoria inválida para receita.");
                return;
            }

            await _transactionRepository.Add(transaction);
            await _unitOfWork.Commit();
        }
    }
}
