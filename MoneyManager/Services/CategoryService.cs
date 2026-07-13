using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.DTOs.Common;
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

    public async Task<ResponseDTO> Create(CreateDto createDto)
    {
        var category = new Category
        {
            Title = createDto.Title,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _repository.Add(category);
        await _repository.SaveChanges();
        return new ResponseDTO()
        {
            Id = category.Id,
            Title = category.Title,
        };
    }

    public async Task<ResponseDTO?> GetById(int id)
    {
        var category = await _repository.GetById(id);
        return category == null
            ? null
            : new ResponseDTO()
            {
                Id = category.Id,
                Title = category.Title,
            };
    }

    public async Task<IEnumerable<ResponseDTO>> GetAll()
    {
        var categories = await _repository.GetAll();
        return categories.Select(c => new ResponseDTO()
        {
            Id = c.Id,
            Title = c.Title,
        }).ToList();
    }

    public async Task<bool> Delete(int id)
    {
        var category = await _repository.GetById(id);
        if (category is null) return false;
        category.DeletedAt = DateTime.UtcNow;
        await _repository.SaveChanges();
        return true;
    }

    public async Task<ServiceResult> Update(UpdateDto updateDto, int id)
    {
        var category = await _repository.GetById(id);
        if (category is null) return ServiceResult.NotFound($"Category not found");
        category.Title = updateDto.Title;
        category.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChanges();
        return ServiceResult.Success();
    }
}