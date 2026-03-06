using ResidentialExpenseControl.Domain.Notifications;
using ResidentialExpenseControl.Domain.Utils.Validations;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public abstract class SearchInput<TOrderBy> : Notifier
    {
        public TOrderBy OrderBy { get; }
        public string OrderDirection { get; }
        public int? PageIndex { get; }
        public int? PageSize { get; }

        protected SearchInput(
            TOrderBy orderBy,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
        {
            OrderBy = orderBy;
            OrderDirection = string.IsNullOrWhiteSpace(orderDirection) ? "ASC" : orderDirection.ToUpperInvariant();
            PageIndex = pageIndex;
            PageSize = pageSize;

            this.Validate();
        }

        public bool HasPagination()
        {
            return this.PageIndex.HasValue && this.PageSize.HasValue;
        }

        protected virtual void Validate()
        {
            if (this.PageIndex.HasValue)
                this.NotifyIfLessThan(this.PageIndex.Value, 1, "Index da paginação é inválido.");

            if (this.PageSize.HasValue)
                this.NotifyIfLessThan(this.PageSize.Value, 1, "Tamanho da página utilizado na paginação é inválido.");
        }
    }

}
