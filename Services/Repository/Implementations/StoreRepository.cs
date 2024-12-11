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
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        private readonly SheyaakaDbContext _context;
        public StoreRepository(SheyaakaDbContext context) : base(context)
        {
            _context = context;
        }
        //get single for delete
        public async Task<int> GetSingleAddressAssociatedWithUserStore(int addressID, int userId)
        {
            return await _context.Database.ExecuteSqlRawAsync(@"
        Select * FROM Addresses 
        WHERE AddressID = {0} 
        AND EXISTS (
            SELECT 1 
            FROM Stores s 
            WHERE s.StoreID = Addresses.StoreID 
            AND s.UserID = {1}
        )",
                addressID, userId);
        }
        public IEnumerable<Address> GetAddressesByStoreAndUser(int storeId, int associatedUserId)
        {
            return _context.Addresses
                .FromSqlRaw(@"SELECT a.* 
                           FROM Addresses a
                           JOIN Stores s ON a.StoreID = s.StoreID
                           WHERE a.StoreID = {0} AND s.UserID = {1}", storeId, associatedUserId)
                .ToList();
        }
        //get single for edit
        public async Task<Address?> GetSingleAddressAssociatedWithUserStoreforEdit(int addressID, int userId)
        {
            return await _context.Addresses
                .FromSqlRaw(@"
            SELECT a.* 
            FROM Addresses a
            WHERE a.AddressID = {0} 
            AND EXISTS (
                SELECT 1 
                FROM Stores s 
                WHERE s.StoreID = a.StoreID 
                AND s.UserID = {1}
            )",
                    addressID, userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(); // Return null if no record is found
        }


        public void GetAssociatedUser(int userId, int storeId)
        {
            //  check if the user is associated with the store
            var userStore = _context.Stores
                .FirstOrDefault(u => u.UserID == userId && u.StoreID == storeId);

            
        }
    }
}
