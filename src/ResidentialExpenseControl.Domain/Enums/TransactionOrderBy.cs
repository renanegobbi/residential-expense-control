using System.ComponentModel;

namespace ResidentialExpenseControl.Domain.Enums
{
    public enum TransactionOrderBy
    {
        [Description("Transaction ID")]
        Id,

        [Description("Transaction description")]
        Description,

        [Description("Transaction amount")]
        Amount,

        [Description("Transaction type")]
        Type,

        [Description("Category ID")]
        CategoryId,

        [Description("Person ID")]
        PersonId
    }
}
