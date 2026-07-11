using MoneyManager.DTOs.Category;
using MoneyManager.Models;

namespace MoneyManager.Services.Interfaces;

public interface ICategoryService
{
    Task<Category> CreateCategory(CreateDto createDto);
}