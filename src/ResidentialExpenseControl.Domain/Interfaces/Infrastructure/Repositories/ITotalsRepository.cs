using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using System;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    /// <summary>
    /// Read-only repository contract for totals reports.
    /// </summary>
    public interface ITotalsRepository
    {
        Task<Tuple<TotalsOutput<TotalsByPersonItemOutput>, double>> GetTotalsByPeople(TotalsFilterByPersonInput input);
        Task<Tuple<TotalsOutput<TotalsByCategoryItemOutput>, double>> GetTotalsByCategory(TotalsFilterByCategoryInput input);
    }
}
