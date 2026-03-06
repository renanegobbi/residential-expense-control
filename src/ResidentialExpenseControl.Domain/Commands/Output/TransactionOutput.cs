using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class TransactionOutput
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public int CategoryId { get; set; }
        public int PersonId { get; set; }
    }
}
