using ResidentialExpenseControl.Domain.Enums;

namespace ResidentialExpenseControl.Domain.Commands.Input
{
    public class SearchPeopleInput : SearchInput<PersonOrderBy>
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? Age { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public SearchPeopleInput(
            PersonOrderBy? orderBy = PersonOrderBy.Id,
            string orderDirection = "ASC",
            int? pageIndex = null,
            int? pageSize = null)
            : base(orderBy ?? PersonOrderBy.Id, orderDirection, pageIndex, pageSize)
        {
        }
    }
}
