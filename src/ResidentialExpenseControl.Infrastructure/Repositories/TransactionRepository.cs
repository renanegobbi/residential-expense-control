using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }

        public async Task<Tuple<Transaction[], double>> GetAll(SearchTransactionInput input)
        {
            IEnumerable<Transaction> records = Db.Transactions.AsNoTracking();

            // Filters
            if (input.Id.HasValue)
                records = records.Where(t => t.Id == input.Id.Value);

            if (!string.IsNullOrWhiteSpace(input.Description))
                records = records.Where(t => t.Description.Contains(input.Description));

            if (input.Amount.HasValue)
                records = records.Where(t => t.Amount == input.Amount.Value);

            if (input.Type.HasValue)
                records = records.Where(t => t.Type == input.Type.Value);

            if (input.CategoryId.HasValue)
                records = records.Where(t => t.CategoryId == input.CategoryId.Value);

            if (input.PersonId.HasValue)
                records = records.Where(t => t.PersonId == input.PersonId.Value);

            switch (input.OrderBy)
            {
                case TransactionOrderBy.Description:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(t => t.Description)
                        : records.OrderBy(t => t.Description);
                    break;
                case TransactionOrderBy.Amount:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(t => t.Amount)
                        : records.OrderBy(t => t.Amount);
                    break;
                case TransactionOrderBy.Type:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(t => t.Type)
                        : records.OrderBy(t => t.Type);
                    break;
                case TransactionOrderBy.CategoryId:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(t => t.CategoryId)
                        : records.OrderBy(t => t.CategoryId);
                    break;
                case TransactionOrderBy.PersonId:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(t => t.PersonId)
                        : records.OrderBy(t => t.PersonId);
                    break;
                default:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(p => p.Id)
                        : records.OrderBy(p => p.Id);
                    break;
            }

            var totalRecords = Convert.ToDouble(records.Count());

            records = records
                .Skip((int)input.PageSize * ((int)input.PageIndex - 1))
                .Take((int)input.PageSize)
                .ToList();

            if (input.HasPagination())
            {
                return new Tuple<Transaction[], double>(records.ToArray(), totalRecords);
            }
            else
            {
                return new Tuple<Transaction[], double>(records.ToArray(), totalRecords);
            }
        }

        public async Task<bool> ExistsByCategoryId(int categoryId)
        {
            return await Db.Transactions.AsNoTracking().AnyAsync(t => t.CategoryId == categoryId);
        }

        public async Task<bool> HasIncomeByPersonId(int personId)
        {
            return await Db.Transactions.AsNoTracking()
                .AnyAsync(t => t.PersonId == personId && t.Type == TransactionType.Income);
        }

    }
}
