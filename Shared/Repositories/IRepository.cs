using Models;
using System.Linq.Expressions;

namespace Sheyaaka.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        /*void Update(T entity);*/
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void update(T Entity);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
        public Task<User> GetByEmail(string email);
        
    }
}
