using Domain.Entities;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByNameAsync(string name);
    Task SaveChangesAsync();
    void Delete(Category category);
}
