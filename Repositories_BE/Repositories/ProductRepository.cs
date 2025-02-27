using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SWP391G3DbContext _context;

        public ProductRepository(SWP391G3DbContext context)
        {
            _context = context;
        }

        public async Task<ProductList> CreateProductAsync(ProductList product)
        {
            product.ProductListId = Guid.NewGuid();
            _context.Set<ProductList>().Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductList> GetProductByIdAsync(Guid productId)
        {
            return await _context.Set<ProductList>().FindAsync(productId);
        }

        public async Task<List<ProductList>> GetAllProductsAsync()
        {
            return await _context.Set<ProductList>().ToListAsync();
        }

        public async Task<ProductList> UpdateProductAsync(ProductList product)
        {
            var existingProduct = await _context.Set<ProductList>().FindAsync(product.ProductListId);
            if (existingProduct == null)
            {
                return null;
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await _context.Set<ProductList>().FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _context.Set<ProductList>().Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
