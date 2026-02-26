namespace ResidentialExpenseControl.Domain.Notifications
{
    /// <summary>
    /// Represents a domain notification used to collect validation
    /// and business rule errors.
    /// </summary>
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Message describing the validation or business rule error.
        /// </summary>
        public string Message { get; }
    }
}
