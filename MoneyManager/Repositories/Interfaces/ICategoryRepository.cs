using MoneyManager.DTOs.Category;
using MoneyManager.Models;

namespace MoneyManager.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category> Add(Category category);
    Task<IEnumerable<Category>> GetAll(int userId);
    Task<Category?> GetById(int id);
    Task SaveChanges();
    Task<bool> ExistsByTitle(string title);
}