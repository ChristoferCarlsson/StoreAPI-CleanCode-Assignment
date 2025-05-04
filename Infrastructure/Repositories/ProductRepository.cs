using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);

    public void Delete(Product product) => _context.Products.Remove(product);

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

    public void Update(Product product) => _context.Products.Update(product);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    // Implemented method to check for existing product by name and category
    public async Task<Product> GetByNameAndCategoryAsync(string name, int categoryId)
    {
        // Query the database for a product with the same name and categoryId
        return await _context.Products
                             .Where(p => p.Name == name && p.CategoryId == categoryId)
                             .FirstOrDefaultAsync();
    }
}
