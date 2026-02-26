using ResidentialExpenseControl.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
    }
}
