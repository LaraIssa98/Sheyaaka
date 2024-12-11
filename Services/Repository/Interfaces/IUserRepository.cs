using Models;
using Sheyaaka.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        public Task<User> GetUserStore(int userId, int storeId);
    }
}
