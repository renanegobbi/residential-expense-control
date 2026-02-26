using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Core.DomainObjects;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Infrastructure.Context;
using System.Linq.Expressions;

namespace ResidentialExpenseControl.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity, new()
    {
        protected readonly ResidentialExpenseControlContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(ResidentialExpenseControlContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}

