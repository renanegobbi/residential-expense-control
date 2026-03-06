using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public class TotalsFilterByPersonInput : SearchInput<TotalsPersonOrderBy>
    {
        public TotalsFilterByPersonInput(
            TotalsPersonOrderBy? orderBy = TotalsPersonOrderBy.PersonId,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
            : base(orderBy ?? TotalsPersonOrderBy.PersonId, orderDirection, pageIndex, pageSize)
        {
        }
    }
}
