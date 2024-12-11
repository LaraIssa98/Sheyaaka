using DataAccess.Data;

namespace Sheyaaka.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SheyaakaDbContext _db;

        public UnitOfWork(SheyaakaDbContext db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
