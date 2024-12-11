using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Repository.Interfaces;
using Sheyaaka.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly SheyaakaDbContext _context;
        public UserRepository(SheyaakaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserStore(int userId, int storeId)
        {
            
            return await _context.Users
                              .Where(us => us.UserID == userId).FirstOrDefaultAsync()?? new();
        }
    }
}
