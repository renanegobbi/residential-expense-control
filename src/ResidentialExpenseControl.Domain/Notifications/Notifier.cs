using ResidentialExpenseControl.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ResidentialExpenseControl.Domain.Notifications
{
    /// <summary>
    /// Handles domain notifications.
    /// </summary>
    public class Notifier : INotifier
    {
        private readonly List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
