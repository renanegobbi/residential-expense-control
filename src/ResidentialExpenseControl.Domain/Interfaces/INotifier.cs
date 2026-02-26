using ResidentialExpenseControl.Domain.Notifications;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
