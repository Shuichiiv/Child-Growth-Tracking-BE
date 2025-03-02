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

        public async Task<bool> CreateProductAsync(ProductList product)
        {
            product.ProductListId = Guid.NewGuid();
            _context.Set<ProductList>().Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productListId)
        {
            var product = await _context.Set<ProductList>().FindAsync(productListId);
            if (product == null)
            {
                return false;
            }

            _context.Set<ProductList>().Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductAsync(ProductList product)
        {
            var existingProduct = await _context.Set<ProductList>().FindAsync(product.ProductListId);
            if (existingProduct == null)
            {
                return false;
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductList>> GetAllProductsAsync()
        {
            return await _context.Set<ProductList>().ToListAsync();
        }

        public async Task<ProductList> GetProductByIdAsync(Guid productListId)
        {
            return await _context.Set<ProductList>().FindAsync(productListId);
        }

    }
}
