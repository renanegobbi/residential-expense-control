using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class TotalsRepository : ITotalsRepository
    {
        private readonly ResidentialExpenseControlContext _db;

        public TotalsRepository(ResidentialExpenseControlContext db) => _db = db;

        public async Task<Tuple<TotalsOutput<TotalsByPersonItemOutput>, double>> GetTotalsByPeople(TotalsFilterByPersonInput input)
        {
            input ??= new TotalsFilterByPersonInput();

            var txAgg = await _db.Transactions
                .AsNoTracking()
                .GroupBy(t => t.PersonId)
                .Select(g => new
                {
                    PersonId = g.Key,
                    Income = g.Where(x => x.Type == TransactionType.Income)
                              .Sum(x => (double)x.Amount),
                    Expense = g.Where(x => x.Type == TransactionType.Expense)
                               .Sum(x => (double)x.Amount)
                })
                .ToListAsync();

            var people = await _db.People
                .AsNoTracking()
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            var itemsQuery = (from p in people
                              join a in txAgg on p.Id equals a.PersonId into pa
                              from a in pa.DefaultIfEmpty()
                              select new TotalsByPersonItemOutput
                              {
                                  PersonId = p.Id,
                                  PersonName = p.Name,
                                  TotalIncome = a == null ? 0m : (decimal)a.Income,
                                  TotalExpense = a == null ? 0m : (decimal)a.Expense
                              })
                             .AsQueryable();

            var totalRecords = Convert.ToDouble(itemsQuery.Count());

            switch (input.OrderBy)
            {
                case TotalsPersonOrderBy.PersonId:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.PersonId)
                        : itemsQuery.OrderBy(x => x.PersonId);
                    break;

                case TotalsPersonOrderBy.PersonName:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.PersonName)
                        : itemsQuery.OrderBy(x => x.PersonName);
                    break;

                case TotalsPersonOrderBy.TotalIncome:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.TotalIncome)
                        : itemsQuery.OrderBy(x => x.TotalIncome);
                    break;

                case TotalsPersonOrderBy.TotalExpense:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.TotalExpense)
                        : itemsQuery.OrderBy(x => x.TotalExpense);
                    break;

                case TotalsPersonOrderBy.Balance:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.Balance)
                        : itemsQuery.OrderBy(x => x.Balance);
                    break;

                default:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.PersonId)
                        : itemsQuery.OrderBy(x => x.PersonId);
                    break;
            }

            var totalIncome = txAgg.Sum(x => (decimal)x.Income);
            var totalExpense = txAgg.Sum(x => (decimal)x.Expense);


            if (input.HasPagination())
            {
                var pageIndex = input.PageIndex ?? 1;
                var pageSize = input.PageSize ?? 10;

                itemsQuery = itemsQuery
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize);
            }

            var pageItems = itemsQuery.ToList();

            var output = new TotalsOutput<TotalsByPersonItemOutput>
            {
                Items = pageItems,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
            };

            return new Tuple<TotalsOutput<TotalsByPersonItemOutput>, double>(output, totalRecords);
        }

        public async Task<Tuple<TotalsOutput<TotalsByCategoryItemOutput>, double>> GetTotalsByCategory(TotalsFilterByCategoryInput input)
        {
            input ??= new TotalsFilterByCategoryInput();

            var txAgg = await _db.Transactions
                .AsNoTracking()
                .GroupBy(t => t.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Income = g.Where(x => x.Type == TransactionType.Income)
                              .Sum(x => (double)x.Amount),
                    Expense = g.Where(x => x.Type == TransactionType.Expense)
                               .Sum(x => (double)x.Amount)
                })
                .ToListAsync();

            var categories = await _db.Categories
                .AsNoTracking()
                .Select(c => new { c.Id, c.Description })
                .ToListAsync();

            var itemsQuery = (from c in categories
                              join a in txAgg on c.Id equals a.CategoryId into ca
                              from a in ca.DefaultIfEmpty()
                              select new TotalsByCategoryItemOutput
                              {
                                  CategoryId = c.Id,
                                  CategoryDescription = c.Description,
                                  TotalIncome = a == null ? 0m : (decimal)a.Income,
                                  TotalExpense = a == null ? 0m : (decimal)a.Expense
                              })
                             .AsQueryable();

            var totalRecords = Convert.ToDouble(itemsQuery.Count());

            var totalIncome = txAgg.Sum(x => (decimal)x.Income);
            var totalExpense = txAgg.Sum(x => (decimal)x.Expense);

            switch (input.OrderBy)
            {
                case TotalsCategoryOrderBy.CategoryId:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.CategoryId)
                        : itemsQuery.OrderBy(x => x.CategoryId);
                    break;

                case TotalsCategoryOrderBy.CategoryDescription:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.CategoryDescription)
                        : itemsQuery.OrderBy(x => x.CategoryDescription);
                    break;

                case TotalsCategoryOrderBy.TotalIncome:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.TotalIncome)
                        : itemsQuery.OrderBy(x => x.TotalIncome);
                    break;

                case TotalsCategoryOrderBy.TotalExpense:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.TotalExpense)
                        : itemsQuery.OrderBy(x => x.TotalExpense);
                    break;

                case TotalsCategoryOrderBy.Balance:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.Balance)
                        : itemsQuery.OrderBy(x => x.Balance);
                    break;

                default:
                    itemsQuery = input.OrderDirection == "DESC"
                        ? itemsQuery.OrderByDescending(x => x.CategoryId)
                        : itemsQuery.OrderBy(x => x.CategoryId);
                    break;
            }

            if (input.HasPagination())
            {
                var pageIndex = input.PageIndex ?? 1;
                var pageSize = input.PageSize ?? 10;

                itemsQuery = itemsQuery
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize);
            }

            var pageItems = itemsQuery.ToList();

            var output = new TotalsOutput<TotalsByCategoryItemOutput>
            {
                Items = pageItems,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            };

            return new Tuple<TotalsOutput<TotalsByCategoryItemOutput>, double>(output, totalRecords);
        }

    }
}
