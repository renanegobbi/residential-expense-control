using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public class SearchTransactionInput : SearchInput<TransactionOrderBy>
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public TransactionType? Type { get; set; }
        public int? CategoryId { get; set; }
        public int? PersonId { get; set; }

        public SearchTransactionInput(
            TransactionOrderBy? orderBy = TransactionOrderBy.Id,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
            : base(orderBy ?? TransactionOrderBy.Id, orderDirection, pageIndex, pageSize)
        {
        }
    }
}
