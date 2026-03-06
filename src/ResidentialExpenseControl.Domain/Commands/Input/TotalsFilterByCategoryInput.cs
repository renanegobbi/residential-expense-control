using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public class TotalsFilterByCategoryInput : SearchInput<TotalsCategoryOrderBy>
    {
        public TotalsFilterByCategoryInput(
            TotalsCategoryOrderBy? orderBy = TotalsCategoryOrderBy.CategoryId,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
            : base(orderBy ?? TotalsCategoryOrderBy.CategoryId, orderDirection, pageIndex, pageSize)
        {
        }
    }
}
