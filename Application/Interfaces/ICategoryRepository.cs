using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        void Delete(Category category);
    }
}
