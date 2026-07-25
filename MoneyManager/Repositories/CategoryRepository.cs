using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;

namespace MoneyManager.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Category> Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        return category;
    }

    public async Task<IEnumerable<Category>> GetAll(int userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId || c.UserId == null)
            .Where(c => c.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<Category?> GetById(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByTitle(string title)
    {
        return await _context.Categories.AnyAsync(c =>
            EF.Functions.Like(c.Title, title) && c.DeletedAt == null);
    }
}