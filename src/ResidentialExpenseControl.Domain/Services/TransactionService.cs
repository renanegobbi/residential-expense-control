using Microsoft.Extensions.Logging;
using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities.Validations;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Extensions;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Utils.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;
using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IPersonRepository personRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<TransactionService> logger) : base(unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IOutput> GetAll(SearchTransactionInput input)
        {
            this.NotifyIfNull(input, "Entrada não informada.");

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            this.AddNotifications(input.Notifications);

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            var result = await _transactionRepository.GetAll(input);

            var itens = result.Item1.Select(x => x.ToTransactionOutput());

            return new SearchOutput(itens, input.OrderBy.ToString(), input.OrderDirection, result.Item2, input.PageIndex, input.PageSize);
        }

        public async Task<TransactionOutput> GetTransactionById(int transactionId)
        {
            var transaction = await _transactionRepository.GetById(transactionId);

            if (transaction == null) return null;

            var transactionOutput = transaction.ToTransactionOutput();

            return transactionOutput;
        }

        public async Task<IOutput> GetTransaction(int transactionId)
        {
            var output = await GetTransactionById(transactionId);

            return output == null
                ? new Output(false, new[] { "Nenhuma transação cadastrada." }, null)
                : new Output(true, new[] { "Transação obtida com sucesso." }, output);
        }

        public async Task<IOutput> Create(Transaction transaction)
        {
            this.NotifyIfNull(transaction, "Entrada não informada.");

            NotifyValidationError(new TransactionValidation(), transaction);
            if (HasNotification())
                return new Output(false, GetNotifications().Select(a => a.Message).ToList(), null);

            var person = await _personRepository.GetById(transaction.PersonId);
            if (person == null)
                return new Output(false, new[] { "Pessoa não encontrada." }, null);

            if (person.Age < 18 && transaction.Type == TransactionType.Income)
                return new Output(false, new[] { "Menores de idade só podem cadastrar despesas." }, null);

            var category = await _categoryRepository.GetById(transaction.CategoryId);
            if (category == null)
                return new Output(false, new[] { "Categoria não encontrada." }, null);

            var allowed =
                category.Purpose == CategoryPurpose.Both ||
                (transaction.Type == TransactionType.Expense && category.Purpose == CategoryPurpose.Expense) ||
                (transaction.Type == TransactionType.Income && category.Purpose == CategoryPurpose.Income);

            if (!allowed)
                return new Output(false, new[] { "A categoria selecionada não é compatível com o tipo da transação." }, null);

            try
            {
                await _transactionRepository.Add(transaction);
                _unitOfWork.Commit();

                var transactionOutput = await GetTransactionById(transaction.Id);

                _logger.LogInformation($"A transação \"{transactionOutput.Id} - {transactionOutput.Description}\" foi cadastrada.");

                return new Output(true, new[] { "Transação cadastrada com sucesso." }, transactionOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cadastrar a transação {transaction.Description}: {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Update(Transaction transaction)
        {
            this.NotifyIfNull(transaction, "Entrada não informada.");

            NotifyValidationError(new TransactionValidation(), transaction);
            if (HasNotification())
                return new Output(false, GetNotifications().Select(a => a.Message).ToList(), null);

            var existingTransaction = await GetTransactionById(transaction.Id);
            if (existingTransaction == null)
                return new Output(false, new[] { "Nenhuma transação encontrada." }, null);

            var person = await _personRepository.GetById(transaction.PersonId);
            if (person == null)
                return new Output(false, new[] { "Pessoa não encontrada." }, null);

            if (person.Age < 18 && transaction.Type == TransactionType.Income)
                return new Output(false, new[] { "Menores de idade só podem cadastrar despesas." }, null);

            var category = await _categoryRepository.GetById(transaction.CategoryId);
            if (category == null)
                return new Output(false, new[] { "Categoria não encontrada." }, null);

            var allowed =
                category.Purpose == CategoryPurpose.Both ||
                (transaction.Type == TransactionType.Expense && category.Purpose == CategoryPurpose.Expense) ||
                (transaction.Type == TransactionType.Income && category.Purpose == CategoryPurpose.Income);

            if (!allowed)
                return new Output(false, new[] { "A categoria selecionada não é compatível com o tipo da transação." }, null);

            try
            {
                _transactionRepository.Update(transaction);
                _unitOfWork.Commit();

                var transactionOutput = await GetTransactionById(transaction.Id);

                _logger.LogInformation($"A transação \"{existingTransaction.Id} - {existingTransaction.Description}\" foi alterada.");

                return new Output(true, new[] { "Transação alterada com sucesso." }, transactionOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar a transação \"{transaction.Id} - {transaction.Description}\": {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Delete(int id)
        {
            this.NotifyIfLessThan(id, 1, "Id inválido.");

            if (HasNotification())
                return new Output(false, this.Messages, null);

            var existingTransaction = await GetTransactionById(id);

            if (existingTransaction == null) return new Output(false, new[] { "Nenhuma transação encontrada" }, null);

            if (HasNotification())
                return new Output(false, this.Messages, null);

            try
            {
                var transaction = existingTransaction.ToTransaction();

                _transactionRepository.Remove(transaction);

                _unitOfWork.Commit();

                _logger.LogInformation($"A transação \"{transaction.Id} - {transaction.Description}\" foi removida.");

                return new Output(true, new[] { "Transação excluída com sucesso." }, transaction.ToTransactionOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover a transação {id}: {ex.Message}");
                throw;
            }
        }
    }
}
