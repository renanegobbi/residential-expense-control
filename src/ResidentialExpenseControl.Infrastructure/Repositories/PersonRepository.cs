using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ResidentialExpenseControlContext db) : base(db)
        {
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await Db.People.AsNoTracking().AnyAsync(p => p.Name == name);
        }
    }
}
