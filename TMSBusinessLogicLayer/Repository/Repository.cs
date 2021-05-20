using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Services;

namespace TMSBusinessLogicLayer.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }
        public async Task<OutputHandler> DeleteByIdAsync(object id)
        {
            var row = await _context.FindAsync<TEntity>(id);

            if (row == null)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "The record does not exist"
                };
            }
            _context.Remove(row);
            await _context.SaveChangesAsync();
            return new OutputHandler
            {
                IsErrorOccured = false,
                Message = "The record has been deleted successfully."
            };
        }
        public void DeleteAsync(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public TEntity GetByIdAsync(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetItem(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetItemWithChildEntity(Expression<Func<TEntity, bool>> expression, string TEntityName)
        {
            IQueryable searchQuery = null;

            if (string.IsNullOrEmpty(TEntityName))
            {
                searchQuery = _context.Set<TEntity>();
            }
            else
            {
                searchQuery = _context.Set<TEntity>().Include(TEntityName);
            }
            try
            {
                var row = await searchQuery.Where(expression).ToDynamicListAsync();
                return row.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }

        public async Task<OutputHandler> SaveChanges()
        {
            var output = await _context.SaveChangesAsync();
            if (output == 0)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true
                };
            }
            else
            {
                return new OutputHandler
                {
                    IsErrorOccured = false
                };
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllWithChildEntity(string entity)
        {
            return await _context.Set<TEntity>().Include(entity).ToListAsync();
            // IQueryable searchQuery = null;
            //searchQuery = _context.Set<TEntity>().Include(entity);
            // var rows = await searchQuery.ToDynamicListAsync<TEntity>();
            // return rows;
        }
        public Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<OutputHandler> CreateAsync(TEntity row)
        {
            var entity = await _context.AddAsync(row);
            return new OutputHandler
            {
                IsErrorOccured = false,
                Message = "Record added successfully",
                Result = row
            };
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await _context.Set<TEntity>()
                       .AnyAsync(expression);

            return result;

        }
    }
}
