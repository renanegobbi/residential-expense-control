using ResidentialExpenseControl.Core.DomainObjects;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Domain.Extensions
{
    public static class TransactionMappingExtensions
    {
        public static TransactionOutput ToTransactionOutput(this Transaction entity)
        {
            if (entity == null) return null;

            return new TransactionOutput
            {
                Id = entity.Id,
                Description = entity.Description,
                Amount = entity.Amount,
                Type = entity.Type,
                CategoryId = entity.CategoryId,
                PersonId = entity.PersonId
            };
        }

        public static Transaction ToTransaction(this TransactionOutput model)
        {
            if (model == null) return null;

            return new Transaction
            {
                Id = model.Id,
                Description = model.Description,
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                PersonId = model.PersonId
            };
        }
    }
}
