using ResidentialExpenseControl.Domain.Commands.Output;
using System.Collections.Generic;
using System.Linq;

namespace ResidentialExpenseControl.Domain.Extensions
{
    public static class TotalsMappingExtensions
    {
        public static TotalsOutput<TItem> ToTotalsOutput<TItem>(
            this IEnumerable<TItem> items,
            decimal totalIncome,
            decimal totalExpense)
        {
            return new TotalsOutput<TItem>
            {
                Items = items ?? Enumerable.Empty<TItem>(),
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            };
        }
    }
}
