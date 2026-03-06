using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }

        public async Task<Tuple<Person[], double>> GetAll(SearchPeopleInput input)
        {
            IEnumerable<Person> records = Db.People.AsNoTracking();

            // Filters
            if (input.Id.HasValue)
                records = records.Where(p => p.Id == input.Id.Value);

            if (!string.IsNullOrWhiteSpace(input.Name))
                records = records.Where(p => p.Name.Contains(input.Name));

            if (input.Age.HasValue)
                records = records.Where(p => p.Age == input.Age.Value);

            if (input.MinAge.HasValue)
                records = records.Where(p => p.Age >= input.MinAge.Value);

            if (input.MaxAge.HasValue)
                records = records.Where(p => p.Age <= input.MaxAge.Value);

            switch (input.OrderBy)
            {
                case PersonOrderBy.Name:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(p => p.Name)
                        : records.OrderBy(p => p.Name);
                    break;
                case PersonOrderBy.Age:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(p => p.Age)
                        : records.OrderBy(p => p.Age);
                    break;
                default:
                    records = input.OrderDirection == "DESC"
                        ? records.OrderByDescending(p => p.Id)
                        : records.OrderBy(p => p.Id);
                    break;
            }

            var totalRecords = Convert.ToDouble(records.Count());

            records = records
                .Skip((int)input.PageSize * ((int)input.PageIndex - 1))
                .Take((int)input.PageSize)
                .ToList();

            if (input.HasPagination())
            {
                return new Tuple<Person[], double>(records.ToArray(), totalRecords);
            }
            else
            {
                return new Tuple<Person[], double>(records.ToArray(), totalRecords);
            }
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await Db.People.AsNoTracking().AnyAsync(p => p.Name == name);
        }

    }
}
