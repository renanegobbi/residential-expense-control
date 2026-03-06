using System.ComponentModel;

namespace ResidentialExpenseControl.Domain.Enums
{
    public enum CategoryOrderBy
    {
        [Description("ID da categoria")]
        Id,

        [Description("Descrição da categoria")]
        Description,

        [Description("Tipo de categoria")]
        Purpose
    }
}
