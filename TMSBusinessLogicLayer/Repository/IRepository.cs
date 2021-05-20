using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Services;

namespace TMSBusinessLogicLayer.Repository
{
    public interface IRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
        IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity GetByIdAsync(int id);
        Task<TEntity> GetItemWithChildEntity(Expression<Func<TEntity, bool>> expression, string TEntityName);
        Task<OutputHandler> DeleteByIdAsync(object id);
        Task<OutputHandler> CreateAsync(TEntity row);
    }
}
