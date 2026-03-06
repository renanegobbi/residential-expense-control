using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Notifications;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Utils.Validations
{
    public static partial class Notify
    {
        /// Adds a notification if the object is null
        public static INotifier NotifyIfNull(this INotifier notifiable, object obj, string message, Dictionary<string, string> additionalInfo = null)
        {
            if (notifiable == null)
                return null;

            if (obj == null)
                notifiable.Handle(new Notification(message));

            return notifiable;
        }
    }
}
