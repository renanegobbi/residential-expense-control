using ResidentialExpenseControl.Api.ViewModels.Transaction;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Api.Extensions
{
    public static class TransactionMappingExtensions
    {
        public static SearchTransactionInput ToSearchTransactionInput(this SearchTransactionViewModel model)
        {
            if (model == null) return null;

            var input = new SearchTransactionInput(
                model.OrderBy,
                model.OrderDirection ?? "ASC",
                model.PageIndex,
                model.PageSize
            )
            {
                Id = model.Id,
                Description = model.Description,
                Amount = model.Amount,
                Type = model.Type,
                CategoryId = model.CategoryId,
                PersonId = model.PersonId
            };

            return input;
        }

        public static Transaction ToTransaction(this CreateTransactionViewModel model)
        {
            if (model == null) return null;

            return new Transaction
            {
                Description = model.Description,
                Amount = model.Amount,
                Type = model.Type,
                CategoryId = model.CategoryId,
                PersonId = model.PersonId   
            };
        }

        public static Transaction ToTransaction(this UpdateTransactionViewModel model)
        {
            if (model == null) return null;

            return new Transaction
            {
                Id = model.Id,
                Description = model.Description,
                Amount = model.Amount,
                Type = model.Type,
                CategoryId = model.CategoryId,
                PersonId = model.PersonId
            };
        }

    }
}
