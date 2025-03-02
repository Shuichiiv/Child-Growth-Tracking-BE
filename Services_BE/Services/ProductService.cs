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

            if (!ProductTypes.All.Contains(model.ProductType))
                throw new ArgumentException("ProductType không hợp lệ");

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
            return await _productRepository.DeleteProductAsync(model.ProducListId);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName))
                throw new ArgumentException("Tên sản phẩm không được để trống");

            if (model.Price <= 0)
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");

            if (!ProductTypes.All.Contains(model.ProductType))
                throw new ArgumentException("ProductType không hợp lệ");


            var product = new ProductList
            {
                ProductListId = model.ProductListId,
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

        public async Task<List<ProductResponseDto>> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(p => new ProductResponseDto
            {
                ProductListId = p.ProductListId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                Price = p.Price,
                MinAge = p.MinAge,
                MaxAge = p.MaxAge,
                SafetyFeature = p.SafetyFeature,
                Rating = p.Rating,
                RecommendedBy = p.RecommendedBy,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand,
                IsActive = p.IsActive,
                ProductType = p.ProductType
            }).ToList();
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(Guid productListId)
        {
            var product = await _productRepository.GetProductByIdAsync(productListId);
            if (product == null) return null;
            return new ProductResponseDto
            {
                ProductListId = product.ProductListId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                MinAge = product.MinAge,
                MaxAge = product.MaxAge,
                SafetyFeature = product.SafetyFeature,
                Rating = product.Rating,
                RecommendedBy = product.RecommendedBy,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand,
                IsActive = product.IsActive,
                ProductType = product.ProductType
            };
        }

    }
    public static class ProductTypes
    {
        public const string Underweight = "Underweight";
        public const string Balanced = "Balanced";
        public const string Overweight = "Overweight";

        public static readonly HashSet<string> All = new HashSet<string>
        {
            Underweight,
            Balanced,
            Overweight
        };
    }
}
