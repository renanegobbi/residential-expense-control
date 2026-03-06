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

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
