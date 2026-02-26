using ResidentialExpenseControl.Core.DomainObjects;
using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Entities
{
    public class Transaction : Entity
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }

        public int CategoryId { get; set; }
        public int PersonId { get; set; }

        /* EF Relations */
        public Category Category { get; set; }
        public Person Person { get; set; }
    }
}
