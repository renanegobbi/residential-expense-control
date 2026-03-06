using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public class SearchCategoryInput : SearchInput<CategoryOrderBy>
    {
        public int? Id { get; set; }

        public string? Description { get; set; }

        public CategoryPurpose? Purpose { get; set; }

        public SearchCategoryInput(
            CategoryOrderBy? orderBy = CategoryOrderBy.Id,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
            : base(orderBy ?? CategoryOrderBy.Id, orderDirection, pageIndex, pageSize)
        {
        }
    }
}
