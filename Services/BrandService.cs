using Models;
using Services.Repository.Interfaces;
using Sheyaaka.Repositories;
using System;

namespace Services
{
    public class BrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CacheService _cacheService;

        public BrandService(
            IBrandRepository brandRepository,
            IStoreRepository storeRepository,
            IUnitOfWork unitOfWork,
            CacheService cacheService)
        {
            _brandRepository = brandRepository;
            _storeRepository = storeRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public Brand CreateBrand(Brand brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));

            // Ensure brand name uniqueness
            if (_brandRepository.Get(b => b.BrandName == brand.BrandName) != null)
                throw new InvalidOperationException("Brand name must be unique.");

            _brandRepository.Add(brand);
            _unitOfWork.Save();
            // Invalidate the cache for the updated brand
            _cacheService.Remove($"Brand_{brand.BrandID}");
            _cacheService.Remove("AllBrands"); // Invalidate the list cache
            return brand;
        }

        public Brand GetBrand(int id)
        {
            /*var brand = _brandRepository.Get(b => b.BrandID == id);
            if (brand == null)
                throw new InvalidOperationException("Brand not found.");
            return brand;*/
            string cacheKey = $"Brand_{id}"; 

            return _cacheService.GetOrAdd(
                cacheKey,
                () => _brandRepository.Get(b => b.BrandID == id),
                TimeSpan.FromMinutes(10)
            ) ?? throw new InvalidOperationException("Brand not found.");
        }

        public void UpdateBrand(int id, Brand brand)
        {
            if (id != brand.BrandID)
                throw new InvalidOperationException("Brand ID mismatch.");

            var existingBrand = _brandRepository.Get(b => b.BrandID == id);
            if (existingBrand == null)
                throw new InvalidOperationException("Brand not found.");
            existingBrand.BrandName = brand.BrandName;

            // Ensure brand name uniqueness when editing
            if (_brandRepository.Get(b => b.BrandName == brand.BrandName && b.BrandID != id) != null)
                throw new InvalidOperationException("Brand name must be unique.");

            _brandRepository.update(existingBrand);
            _unitOfWork.Save();
            // Invalidate the cache for the updated brand
            _cacheService.Remove($"Brand_{id}");
            _cacheService.Remove("AllBrands"); // Invalidate the list cache
        }

        public void DeleteBrand(int id)
        {
            var brand = _brandRepository.Get(b => b.BrandID == id);
            if (brand == null)
                throw new InvalidOperationException("Brand not found.");

            _brandRepository.Remove(brand);
            _unitOfWork.Save();
            // Invalidate the cache for the updated brand
            _cacheService.Remove($"Brand_{id}");
            _cacheService.Remove("AllBrands"); // Invalidate the list cache
        }

        public void UnassignBrandFromStore(int storeId, int brandId)
        {
            var store = _storeRepository.Get(s => s.StoreID == storeId);
            if (store == null)
                throw new InvalidOperationException("Store not found.");

            var brand = _brandRepository.Get(b => b.BrandID == brandId);
            if (brand == null)
                throw new InvalidOperationException("Brand not found.");

            store.BrandID = null;  // Un-assign the brand from the store
            _storeRepository.update(store);
            _unitOfWork.Save();
            // Invalidate the cache for the updated brand
            _cacheService.Remove($"Brand_{brandId}");
            _cacheService.Remove("AllBrands"); // Invalidate the list cache
        }

        public List<Brand> GetAll()
        {
            return _cacheService.GetOrAdd(
        "AllBrands",
        () => _brandRepository.GetAll().ToList(),
        TimeSpan.FromMinutes(10)
    );
        }
    }
}
