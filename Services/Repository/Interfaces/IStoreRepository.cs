using Models;
using Sheyaaka.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IStoreRepository : IRepository<Store>
    {
        void GetAssociatedUser(int userId, int storeId);
        Task<int> GetSingleAddressAssociatedWithUserStore(int addressID, int userId);
        public IEnumerable<Address> GetAddressesByStoreAndUser(int storeId, int associatedUserId);//new
        Task<Address> GetSingleAddressAssociatedWithUserStoreforEdit(int addressID, int userId);
    }
}
