using ResidentialExpenseControl.Core.DomainObjects;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Entities
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }

        /* EF Relations */
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
