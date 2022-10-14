using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService
    {
        public readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return Id, Name, Available and Price fields of the Product, so used ProductModel for this purpose.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductModel>?> GetProductsAsync()
        {
            try
            {
                List<ProductModel> list = await _context.Products
                    .Select(p => new ProductModel() { Id = p.Id, Name = p.Name, Available = p.Available, Price = p.Price })
                    .ToListAsync();
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get product via given Id.
        /// </summary>
        /// <param name="id">Product Id which need to be read from database.</param>
        /// <returns></returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
