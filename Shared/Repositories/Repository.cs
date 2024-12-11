using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

namespace Sheyaaka.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SheyaakaDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(SheyaakaDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate).ToList();
        }

        public async Task<User> GetByEmail(string? email)
        {
            return await _db.Users.Where(u => u.Email == email && u.IsEmailConfirmed == true).FirstOrDefaultAsync()?? new User();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void update(T Entity)
        {
            //_db.Categories.Update(obj);
            dbSet.Update(Entity);
        }
    }
}
