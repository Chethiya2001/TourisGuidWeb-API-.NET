using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repositories
{
    public interface IRepository<P,EID>  where P:IEntity<EID>
    {
        Task CreateAsync(P data);

        Task<IReadOnlyCollection<P>> GetAllAsync(Expression<Func<P, bool>>? filter = null);
        Task<P> GetAsync(EID id);
        Task<P> GetAsync(Expression<Func<P, bool>> filter);
        Task RemoveAsync(EID id);
        Task UpdateAsync(P data);
        Task<IEnumerable<P>> FindAsync(Expression<Func<P, bool>> predicate);
        IQueryable<P> GetAllQueryable();
    }
}