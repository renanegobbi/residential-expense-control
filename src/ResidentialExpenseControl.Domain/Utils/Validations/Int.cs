using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Notifications;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Utils.Validations
{
    public static partial class Notify
    {
        /// Adds a notification if a given number is less than another.
        public static INotifier NotifyIfLessThan(this INotifier notifiable, int number, int comparedNumber, string message, Dictionary<string, string> additionalInfo = null)
        {
            if (notifiable == null)
                return null;

            if (number < comparedNumber)
                notifiable.Handle(new Notification(message));

            return notifiable;
        }
    }
}
