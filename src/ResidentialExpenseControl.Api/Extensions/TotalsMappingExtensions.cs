using ResidentialExpenseControl.Api.ViewModels.Totals;
using ResidentialExpenseControl.Domain.Commands.Input;

namespace ResidentialExpenseControl.Api.Extensions
{
    public static class TotalsMappingExtensions
    {
        public static TotalsFilterByPersonInput ToTotalsFilterInput(this TotalsFilterByPeopleViewModel model)
        {
            if (model == null) return null;

            var input = new TotalsFilterByPersonInput(
                model.OrderBy,
                model.OrderDirection ?? "ASC",
                model.PageIndex,
                model.PageSize
            )
            {
            };

            return input;
        }

        public static TotalsFilterByCategoryInput ToTotalsFilterInput(this TotalsFilterByCategoryViewModel model)
        {
            if (model == null) return null;

            var input = new TotalsFilterByCategoryInput(
                model.OrderBy,
                model.OrderDirection ?? "ASC",
                model.PageIndex,
                model.PageSize
            )
            {
            };

            return input;
        }
    }
}
