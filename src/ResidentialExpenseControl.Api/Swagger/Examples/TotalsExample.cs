using ResidentialExpenseControl.Api.ViewModels.Totals;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Linq;

namespace ResidentialExpenseControl.Api.Swagger.Examples
{
    #region TotalsByPeople

    public class TotalsByPeopleRequesttExample : IExamplesProvider<TotalsFilterByPeopleViewModel>
    {
        public TotalsFilterByPeopleViewModel GetExamples() => new TotalsFilterByPeopleViewModel
        {
            OrderBy = TotalsPersonOrderBy.PersonId,
            OrderDirection = "ASC",
            PageIndex = 1,
            PageSize = 5
        };
    }

    public class TotalsByPeopleResponseExample : IExamplesProvider<TotalsFilterOutput>
    {
        public TotalsFilterOutput GetExamples()
        {
            var data = new List<TotalsByPersonItemOutput>
            {
                new TotalsByPersonItemOutput
                {
                    PersonId = 1,
                    PersonName = "Ana",
                    TotalIncome = 2500.00m,
                    TotalExpense = 1800.00m
                },
                new TotalsByPersonItemOutput
                {
                    PersonId = 2,
                    PersonName = "Bruno",
                    TotalIncome = 3200.00m,
                    TotalExpense = 900.00m
                },
                new TotalsByPersonItemOutput
                {
                    PersonId = 3,
                    PersonName = "Carla",
                    TotalIncome = 0.00m,
                    TotalExpense = 450.00m
                }
            };

            var summary = new TotalsSummaryOutput
            {
                TotalIncome = 5700.00m,
                TotalExpense = 3150.00m
            };

            return new TotalsFilterOutput(
                data: data.Cast<object>(),
                summary: summary,
                orderBy: TotalsPersonOrderBy.PersonId.ToString(),
                orderDirection: "ASC",
                totalRecords: 20,
                pageIndex: 1,
                pageSize: 5
            );
        }
    }
    #endregion

    #region TotalsByCategory

    public class TotalsByCategoryRequestExample : IExamplesProvider<TotalsFilterByCategoryViewModel>
    {
        public TotalsFilterByCategoryViewModel GetExamples() => new TotalsFilterByCategoryViewModel
        {
            OrderBy = TotalsCategoryOrderBy.CategoryId,
            OrderDirection = "ASC",
            PageIndex = 1,
            PageSize = 5
        };
    }

    public class TotalsByCategoryResponseExample : IExamplesProvider<TotalsFilterOutput>
    {
        public TotalsFilterOutput GetExamples()
        {
            var data = new List<TotalsByCategoryItemOutput>
            {
                new TotalsByCategoryItemOutput
                {
                    CategoryId = 1,
                    CategoryDescription = "Alimentação",
                    TotalIncome = 0.00m,
                    TotalExpense = 350.50m
                },
                new TotalsByCategoryItemOutput
                {
                    CategoryId = 2,
                    CategoryDescription = "Salário",
                    TotalIncome = 5000.00m,
                    TotalExpense = 0.00m
                },
                new TotalsByCategoryItemOutput
                {
                    CategoryId = 3,
                    CategoryDescription = "Lazer",
                    TotalIncome = 0.00m,
                    TotalExpense = 200.00m
                }
            };

            var summary = new TotalsSummaryOutput
            {
                TotalIncome = 5250.00m,
                TotalExpense = 550.50m
            };

            return new TotalsFilterOutput(
                data: data.Cast<object>(),
                summary: summary,
                orderBy: TotalsCategoryOrderBy.CategoryId.ToString(),
                orderDirection: "ASC",
                totalRecords: 12,
                pageIndex: 1,
                pageSize: 5
            );
        }
    }

    #endregion
}


