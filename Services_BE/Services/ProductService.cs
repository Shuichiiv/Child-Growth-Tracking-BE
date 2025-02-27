using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;
using Services_BE.Interfaces;
using Repositories_BE.Interfaces;

namespace Services_BE.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductList> CreateProductAsync(ProductList product)
        {
            // Kiểm tra điều kiện
            if (string.IsNullOrEmpty(product.ProductName))
                throw new ArgumentException("Tên sản phẩm không được để trống");

            if (product.Price <= 0)
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");

            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<ProductList> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException("Không tìm thấy sản phẩm");

            return product;
        }

        public async Task<List<ProductList>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<ProductList> UpdateProductAsync(ProductList product)
        {
            // Kiểm tra điều kiện
            if (string.IsNullOrEmpty(product.ProductName))
                throw new ArgumentException("Tên sản phẩm không được để trống");

            if (product.Price <= 0)
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");

            var existingProduct = await _productRepository.GetProductByIdAsync(product.ProductListId);
            if (existingProduct == null)
                throw new KeyNotFoundException("Không tìm thấy sản phẩm");

            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
                throw new KeyNotFoundException("Không tìm thấy sản phẩm");

            return await _productRepository.DeleteProductAsync(productId);
        }

    }
}
