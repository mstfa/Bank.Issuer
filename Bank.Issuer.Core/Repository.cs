using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bank.Issuer.Data;
using Bank.Issuer.Data.Extensions;
using Bank.Issuer.Library.Entities.Base;
using Bank.Issuer.Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Issuer.Core
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BankIssuerDbContext _context;

        public Repository(BankIssuerDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var result = Queryable.Where<T>(_context.Set<T>(), i => true);
            return await result.Include(includes).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var result = Queryable.Where(_context.Set<T>(), where);
            return await result.Include(includes).ToListAsync();
        }
        public async Task<T> GetAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var result = Queryable.Where<T>(_context.Set<T>(), x => x.Id == id);

            return await result.Include(includes).FirstOrDefaultAsync();
        }
        public async Task<Guid> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public Guid Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
