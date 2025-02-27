using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> CreateProductAsync(ProductList product);
        Task<bool> DeleteProductAsync(Guid productId);
        Task<bool> UpdateProductAsync(ProductList product);
        Task<List<ProductList>> GetAllProductsAsync();
        Task<ProductList> GetProductByIdAsync(Guid productId);


    }
}
