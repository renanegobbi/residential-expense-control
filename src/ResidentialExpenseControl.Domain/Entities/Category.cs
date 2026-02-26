using ResidentialExpenseControl.Core.DomainObjects;
using ResidentialExpenseControl.Domain.Enums;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Entities
{
    public class Category : Entity
    {
        public string Description { get; set; }
        public CategoryPurpose Purpose { get; set; }

        /* EF Relations */
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
