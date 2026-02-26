namespace ResidentialExpenseControl.Domain.Interfaces.Commands.Input
{
    /// <summary>
    /// Base interface to standardize domain inputs.
    /// Inherits from INotifier to collect validation/business notifications.
    /// </summary>
    public interface IInput : INotifier
    {
    }
}
