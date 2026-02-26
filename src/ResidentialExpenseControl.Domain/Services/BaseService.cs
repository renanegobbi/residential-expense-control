using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Notifications;

namespace ResidentialExpenseControl.Domain.Services
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool IsValid()
        {
            return !_notifier.HasNotification();
        }
    }
}
