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
    private readonly ICurrentUserService _currentUser;

    public CategoryService(ICategoryRepository repository, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<ResponseDTO>> Create(CreateDto createDto)
    {
        var titleExists = await _repository.ExistsByTitle(createDto.Title);
        if (titleExists)
            return ServiceResult<ResponseDTO>.Failure($"A category with the title {createDto.Title} already exists.");
        var category = new Category
        {
            Title = createDto.Title,
            UserId = _currentUser.GetUserId(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _repository.Add(category);
        await _repository.SaveChanges();
        var resDto = new ResponseDTO()
        {
            Id = category.Id,
            Title = category.Title,
        };
        return ServiceResult<ResponseDTO>.Success(data: resDto);
    }

    public async Task<ServiceResult<ResponseDTO>> GetById(int id)
    {
        var category = await _repository.GetById(id);
        if (category is null) return ServiceResult<ResponseDTO>.NotFound($"Category not found");
        var res = new ResponseDTO()
        {
            Id = category.Id,
            Title = category.Title,
        };
        return ServiceResult<ResponseDTO>.Success(res);
    }

    public async Task<ServiceResult<IEnumerable<ResponseDTO>>> GetAll()
    {
        var categories = await _repository.GetAll(userId: _currentUser.GetUserId());
        var res = categories.Select(c => new ResponseDTO()
        {
            Id = c.Id,
            Title = c.Title,
        }).ToList();
        return ServiceResult<IEnumerable<ResponseDTO>>.Success(res);
    }

    public async Task<bool> Delete(int id)
    {
        var category = await _repository.GetById(id);
        if (category is null) return false;
        category.DeletedAt = DateTime.UtcNow;
        await _repository.SaveChanges();
        return true;
    }

    public async Task<ServiceResult<ResponseDTO>> Update(UpdateDto updateDto, int id)
    {
        var category = await _repository.GetById(id);
        if (category is null) return ServiceResult<ResponseDTO>.NotFound($"Category not found");
        var titleExists = await _repository.ExistsByTitle(updateDto.Title);

        if (titleExists &&
            !string.Equals(category.Title, updateDto.Title))
            return ServiceResult<ResponseDTO>.Failure($"Category with the title {updateDto.Title} already exists.");

        category.Title = updateDto.Title;
        category.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChanges();
        var resDto = new ResponseDTO
        {
            Id = category.Id,
            Title = category.Title,
        };
        return ServiceResult<ResponseDTO>.Success(resDto);
    }

    public async Task<ServiceResult<IEnumerable<SummaryDTO>>> GetSummary(SummaryQueryDTO summaryQuery)
    {
        int currentUserId = _currentUser.GetUserId();
        var summaryData = await _repository.GetSummary(userId: currentUserId, summaryQuery: summaryQuery);
        return ServiceResult<IEnumerable<SummaryDTO>>.Success(summaryData);
    }
}