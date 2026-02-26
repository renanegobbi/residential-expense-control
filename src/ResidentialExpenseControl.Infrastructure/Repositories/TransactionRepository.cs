using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }

        public async Task<List<Transaction>> GetAllWithDetails()
        {
            return await Db.Transactions
                .AsNoTracking()
                .Include(t => t.Person)
                .Include(t => t.Category)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByPersonId(int personId)
        {
            return await Db.Transactions
                .AsNoTracking()
                .Where(t => t.PersonId == personId)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByCategoryId(int categoryId)
        {
            return await Db.Transactions
                .AsNoTracking()
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
