using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;
using DTOs_BE.ProductDTOs;

namespace Services_BE.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(CreateProductModel model);
        Task<bool> DeleteProductAsync(DeleteProductModel model);
        Task<bool> UpdateProductAsync(UpdateProductModel model);
        Task<List<ProductResponseDto>> GetAllProductAsync();
        Task<ProductResponseDto> GetProductByIdAsync(Guid productListId);
        Task<List<ProductResponseDto>> GetProductsByTypeAsync(string productType);

        // Tìm kiếm sản phẩm theo tên
        Task<List<ProductResponseDto>> SearchProductByNameAsync(string productName);

    }
}
