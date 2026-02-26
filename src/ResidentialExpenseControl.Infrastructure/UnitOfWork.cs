using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ResidentialExpenseControlContext _context;

        public UnitOfWork(ResidentialExpenseControlContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
