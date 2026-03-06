using System.ComponentModel;

namespace ResidentialExpenseControl.Domain.Enums
{
    public enum TotalsPersonOrderBy
    {
        [Description("ID da pessoa")]
        PersonId,

        [Description("Nome da pessoa")]
        PersonName,

        [Description("Receita total")]
        TotalIncome,

        [Description("Despesa total")]
        TotalExpense,

        [Description("Saldo")]
        Balance
    }
}

