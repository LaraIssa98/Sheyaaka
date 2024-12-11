using Microsoft.EntityFrameworkCore;
using Models;
using Services.Repository.Interfaces;
using Sheyaaka.Repositories;
using System;
using System.Linq;

namespace Services
{
    public class StoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CacheService _cacheService;

        public StoreService(
            IStoreRepository storeRepository,
            IBrandRepository brandRepository,
            IRepository<Address> addressRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            CacheService cacheService)
        {
            _storeRepository = storeRepository;
            _brandRepository = brandRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public Store CreateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            // Check if the store name is unique
            if (_storeRepository.GetAll().Any(s => s.StoreName == store.StoreName))
                throw new InvalidOperationException("Store name must be unique.");

            // Check if the brand is valid
            if (!_brandRepository.GetAll().Any(b => b.BrandID == store.BrandID))
                throw new InvalidOperationException("Invalid Brand ID.");
            _storeRepository.Add(store);
            _unitOfWork.Save();
            _cacheService.Remove("AllStores");
            return store;
        }

        public Store GetStore(int id)
        {
            var store = _storeRepository.Get(s => s.StoreID == id);
            if (store == null)
                throw new InvalidOperationException("Store not found.");
            return store;
        }
        private bool IsUserAssociatedWithStore(int userId, int storeId)
        {
            _storeRepository.GetAssociatedUser(userId, storeId);
            return true;

        }
        public void UpdateStore(int id, Store store)
        {
            if (id != store.StoreID)
                throw new InvalidOperationException("Store ID mismatch.");

            // Check if the store name is unique
            if (_storeRepository.GetAll().Any(s => s.StoreName == store.StoreName && s.StoreID != id))
                throw new InvalidOperationException("Store name must be unique.");

            // Check if the brand is valid
            if (!_brandRepository.GetAll().Any(b => b.BrandID == store.BrandID))
                throw new InvalidOperationException("Invalid Brand ID.");

            _storeRepository.update(store);
            _unitOfWork.Save();
        }

        public void DeleteStore(int id)
        {
            var store = _storeRepository.Get(s => s.StoreID == id);
            if (store == null)
                throw new InvalidOperationException("Store not found.");

            // Check if there are any addresses linked to the store
            var addresses = _addressRepository.GetAll().Where(a => a.StoreID == id).ToList();
            if (addresses.Any())
                throw new InvalidOperationException("Cannot delete store because there are addresses associated with it.");

            _storeRepository.Remove(store);
            _unitOfWork.Save();
        }

        public Address AddAddress(Address address)
        {
            var store = _storeRepository.Get(s => s.StoreID == address.StoreID);
            if (store == null)
                throw new InvalidOperationException("Store not found.");

            _addressRepository.Add(address);
            _unitOfWork.Save();
            return address;
        }

        public IEnumerable<Address> GetAllAddresses(int storeId, int associatedUser)
        {
            // Fetch addresses in a single query, validating store ownership in the process
            var addresses = _storeRepository.GetAddressesByStoreAndUser(storeId, associatedUser);


            if (!addresses.Any())
                throw new KeyNotFoundException("No addresses found for the specified store and user.");

            return addresses.ToList();
        }
        //single address
        public async Task<int> GetSingleAddress(int addressID, int userId)
        {
            //var dbAddress = _addressRepository.Get(p => p.AddressID == addressID);
            var address = await _storeRepository.GetSingleAddressAssociatedWithUserStore(addressID, userId);
            return address;
        }

        public async Task UpdateAddress( Address updatedAddress, int userId)
        {
            if (updatedAddress == null)
                throw new ArgumentNullException(nameof(updatedAddress));

            // Fetch the address and ensure it belongs to a store owned by the user
            var existingAddress = await _storeRepository.GetSingleAddressAssociatedWithUserStoreforEdit(updatedAddress.AddressID, userId);
            if (existingAddress == null)
                throw new UnauthorizedAccessException("Address not found or you do not have permission to update it.");

            // Update address properties
            existingAddress.StoreID = updatedAddress.StoreID;
            existingAddress.AddressLine = updatedAddress.AddressLine;
            existingAddress.City = updatedAddress.City;
            existingAddress.State = updatedAddress.State;
            existingAddress.ZipCode = updatedAddress.ZipCode;
            existingAddress.IsActive = updatedAddress.IsActive;

            // Save changes
            _addressRepository.update(existingAddress);
             _unitOfWork.Save();
        }
        public async Task DeleteAddress(int addressID, int userId)
        {
            
            var existingAddress = await _storeRepository.GetSingleAddressAssociatedWithUserStoreforEdit(addressID, userId);
            if (existingAddress == null)
                throw new UnauthorizedAccessException("Address not found or you do not have permission to delete it.");

            
            _addressRepository.Remove(existingAddress);

           
            _unitOfWork.Save();
        }

        public Task GetUserStore(int userId, int storeId)
        {
            var associatedUser = IsUserAssociatedWithStore(userId, storeId);
            if (associatedUser)
            {
                
            }
            return _userRepository.GetUserStore(userId, storeId);
        }
    }
}
