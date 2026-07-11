using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;

namespace MoneyManager.Repositories;

public class CategoryRepository:ICategoryRepository
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

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Categories.Where(c => c.DeletedAt == null)
            .ToListAsync();
        
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}