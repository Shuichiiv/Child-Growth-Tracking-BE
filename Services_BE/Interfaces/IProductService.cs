using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;

namespace Services_BE.Interfaces
{
    public interface IProductService
    {
        Task<ProductList> CreateProductAsync(ProductList product);
        Task<ProductList> GetProductByIdAsync(Guid productId);
        Task<List<ProductList>> GetAllProductsAsync();
        Task<ProductList> UpdateProductAsync(ProductList product);
        Task<bool> DeleteProductAsync(Guid productId);

    }
}
