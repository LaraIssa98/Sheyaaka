using Models;
using Services;
using Services.Repository.Interfaces;
using Sheyaaka.Repositories;
using System;
using System.Collections.Generic;

namespace Sheyaaka.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CacheService _cacheService;

        public ProductService(
            IProductRepository productRepository,
            IStoreRepository storeRepository,
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,CacheService cacheService)
        {
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public Product CreateProduct(Product product)
        {
            if (_storeRepository.Get(s => s.StoreID == product.StoreID) == null)
            {
                throw new ArgumentException("Invalid Store ID.");
            }

            if (_brandRepository.Get(b => b.BrandID == product.BrandID) == null)
            {
                throw new ArgumentException("Invalid Brand ID.");
            }

            _productRepository.Add(product);
            _unitOfWork.Save();
            _cacheService.Remove("AllProducts");
            return product;
        }

        public Product GetProduct(int id)        
        {
            // Cache product by ID
            string cacheKey = $"Product_{id}";

            return _cacheService.GetOrAdd(cacheKey, () =>
            {
                var product = _productRepository.Get(p => p.ProductID == id);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found.");
                }
                return product;
            }, TimeSpan.FromMinutes(10));
        }

        public Product UpdateProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                throw new ArgumentException("Product ID mismatch.");
            }

            var existingProduct = _productRepository.Get(p => p.ProductID == id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            if (_storeRepository.Get(s => s.StoreID == product.StoreID) == null)
            {
                throw new ArgumentException("Invalid Store ID.");
            }

            if (_brandRepository.Get(b => b.BrandID == product.BrandID) == null)
            {
                throw new ArgumentException("Invalid Brand ID.");
            }
            existingProduct.StoreID = product.StoreID;
            existingProduct.BrandID = product.BrandID;
            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            existingProduct.ImageURL = product.ImageURL;
            existingProduct.IsDeleted = product.IsDeleted;
            _productRepository.update(existingProduct);
            _unitOfWork.Save();
            _cacheService.Remove($"Product_{id}");
            _cacheService.Remove("AllProducts");
            return product;
        }

        public Product DeleteProduct(int id)
        {
            var product = _productRepository.Get(p => p.ProductID == id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            product.IsDeleted = true;
            _productRepository.update(product);
            _unitOfWork.Save();
            _cacheService.Remove($"Product_{id}");
            _cacheService.Remove("AllProducts");
            return product;
        }

        public Product RecoverProduct(int id)
        {
            var product = _productRepository.Get(p => p.ProductID == id && p.IsDeleted == true);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found or not deleted.");
            }

            product.IsDeleted = false;
            _productRepository.update(product);
            _unitOfWork.Save();
            _cacheService.Remove($"Product_{id}");
            _cacheService.Remove("AllProducts");
            return product;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            // Cache all products
            return _cacheService.GetOrAdd("AllProducts", () =>
            {
                return _productRepository.GetAll(p => p.IsDeleted == false).ToList();
            }, TimeSpan.FromMinutes(5));
        }
    }
}
