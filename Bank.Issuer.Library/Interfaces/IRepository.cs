using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<Guid> InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
        Guid Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
