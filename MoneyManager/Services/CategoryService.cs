using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    
    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Category> CreateCategory(CreateDto createDto)
    {
        var category = new Category
        {
            Title = createDto.Title,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _repository.Add(category);
        await _repository.SaveChanges();
        return category;    
    }
}