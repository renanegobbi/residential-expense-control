using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Notifications;

namespace ResidentialExpenseControl.Domain.Services
{
    public abstract class BaseService : Notifier
    {
        protected readonly IUnitOfWork _uow;

        protected BaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}
