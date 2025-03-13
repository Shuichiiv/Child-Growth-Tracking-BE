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
        //thay vì xóa data thì chỉ cần set IsActive = false
        public async Task<bool> DeleteProductAsync(DeleteProductModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductListId);
            if (product == null)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm");
            }
            product.IsActive = false;
            return await _productRepository.UpdateProductAsync(product);
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

        public async Task<List<ProductResponseDto>> GetProductsByTypeAsync(string productType) 
        {
            if (!ProductTypes.All.Contains(productType))
                throw new ArgumentException("ProductType không hợp lệ");
            var products = await _productRepository.GetProductsByTypeAsync(productType);
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

        // Tìm kiếm sản phẩm theo tên
        public async Task<List<ProductResponseDto>> SearchProductByNameAsync(string productName)
        {
            var products = await _productRepository.SearchProductByNameAsync(productName);
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


    }
    public static class ProductTypes
    {
        public const string SeverelyUnderweight = "Gầy độ III (Rất gầy) - Nguy cơ cao";
        public const string ModeratelyUnderweight = "Gầy độ II - Nguy cơ vừa";
        public const string MildlyUnderweight = "Gầy độ I - Nguy cơ thấp";
        public const string NormalWeight = "Cân nặng bình thường - Bình thường";
        public const string Overweight = "Thừa cân - Nguy cơ tăng nhẹ";
        public const string ObeseClassI = "Béo phì độ I - Nguy cơ trung bình";
        public const string ObeseClassII = "Béo phì độ II - Nguy cơ cao";
        public const string ObeseClassIII = "Béo phì độ III - Nguy cơ rất cao";

        public static readonly HashSet<string> All = new HashSet<string>
        {
            SeverelyUnderweight,    // (bmi < 16.0) Gầy độ III (Rất gầy) - Nguy cơ cao
            ModeratelyUnderweight,  // (bmi < 16.9) Gầy độ II - Nguy cơ vừa
            MildlyUnderweight,      // (bmi < 18.4) Gầy độ I - Nguy cơ thấp
            NormalWeight,           // (bmi < 24.9) Cân nặng bình thường - Bình thường
            Overweight,             // (bmi < 29.9) Thừa cân - Nguy cơ tăng nhẹ
            ObeseClassI,            // (bmi < 34.9) Béo phì độ I - Nguy cơ trung bình
            ObeseClassII,           // (bmi < 39.9) Béo phì độ II - Nguy cơ cao
            ObeseClassIII           // (bmi > 39.9) Béo phì độ III - Nguy cơ rất cao
        
        };
    }
}
