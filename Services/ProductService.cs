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

        /// <summary>
        /// Create product in Database.
        /// </summary>
        /// <param name="product">Product to be created via API.</param>
        /// <returns></returns>
        public async Task<bool> CreateProductAsync(Product product)
        {
            if (product != null)
            {
                try
                {
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update product in Database.
        /// </summary>
        /// <param name="product">Product to be updated via API.</param>
        /// <returns></returns>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product != null)
            {
                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Delete Product from Database
        /// </summary>
        /// <param name="product">Product to be deleted.</param>
        /// <returns></returns>
        public async Task<bool> DeleteProductAsync(Product product)
        {
            if (product != null)
            {
                try
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Delete Product from Database
        /// </summary>
        /// <param name="id">Product Id to be deleted.</param>
        /// <returns></returns>
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                try
                {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
