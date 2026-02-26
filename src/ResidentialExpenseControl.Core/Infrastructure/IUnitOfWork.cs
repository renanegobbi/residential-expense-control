namespace ResidentialExpenseControl.Core.Infrastructure
{
    /// <summary>
    /// Defines a Unit of Work pattern contract.
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
