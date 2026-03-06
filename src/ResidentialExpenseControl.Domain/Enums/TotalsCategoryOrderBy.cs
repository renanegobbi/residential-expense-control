using System.ComponentModel;

namespace ResidentialExpenseControl.Domain.Enums
{
    public enum TotalsCategoryOrderBy
    {
        [Description("ID da categoria")]
        CategoryId,

        [Description("Descrição da categoria")]
        CategoryDescription,

        [Description("Receita total")]
        TotalIncome,

        [Description("Despesa total")]
        TotalExpense,

        [Description("Saldo")]
        Balance
    }
}

