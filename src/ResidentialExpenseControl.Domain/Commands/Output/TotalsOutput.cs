using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class TotalsSummaryOutput
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }

    public class TotalsOutput<TItem>
    {
        public IEnumerable<TItem> Items { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }

    public class TotalsByPersonItemOutput
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }

    public class TotalsByCategoryItemOutput
    {
        public int CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }
}
