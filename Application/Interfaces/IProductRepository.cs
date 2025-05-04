// Application/Interfaces/IProductRepository.cs
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        void Delete(Product product);
        Task<List<Product>> GetAllAsync(); // Add this method
        Task<Product?> GetByIdAsync(int id);
        void Update(Product product);
        Task SaveChangesAsync();
        Task<Product> GetByNameAndCategoryAsync(string name, int categoryId);
    }
}
