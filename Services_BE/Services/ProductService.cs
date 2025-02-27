using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;
using Services_BE.Interfaces;
using Repositories_BE.Interfaces;
using DTOs_BE.ProductDTOs;

namespace Services_BE.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CreateProductAsync(CreateProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName))
                throw new ArgumentException("Tên sản phẩm không được để trống");

            if (model.Price <= 0)
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");

            var product = new ProductList
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Price = model.Price,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge,
                SafetyFeature = model.SafetyFeature,
                Rating = model.Rating,
                RecommendedBy = model.RecommendedBy,
                ImageUrl = model.ImageUrl,
                Brand = model.Brand,
                IsActive = model.IsActive,
                ProductType = model.ProductType
            };

            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(DeleteProductModel model)
        {
            return await _productRepository.DeleteProductAsync(model.ProductId);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName))
                throw new ArgumentException("Tên sản phẩm không được để trống");

            if (model.Price <= 0)
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");

            var product = new ProductList
            {
                ProductListId = model.ProductId,
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Price = model.Price,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge,
                SafetyFeature = model.SafetyFeature,
                Rating = model.Rating,
                RecommendedBy = model.RecommendedBy,
                ImageUrl = model.ImageUrl,
                Brand = model.Brand,
                IsActive = model.IsActive,
                ProductType = model.ProductType
            };

            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<List<ProductList>> GetAllProductAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<ProductList> GetProductByIdAsync(Guid productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

    }
}
