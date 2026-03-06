using System.ComponentModel;

namespace ResidentialExpenseControl.Domain.Enums
{
    public enum PersonOrderBy
    {
        [Description("ID da pessoa")]
        Id,

        [Description("Nome da pessoa")]
        Name,

        [Description("Idade da pessoa")]
        Age
    }
}
