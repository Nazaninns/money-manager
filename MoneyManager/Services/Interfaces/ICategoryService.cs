using MoneyManager.DTOs.Category;
using MoneyManager.Models;

namespace MoneyManager.Services.Interfaces;

public interface ICategoryService
{
    Task<ResponseDTO> Create(CreateDto createDto);
    Task<ResponseDTO?> GetById(int id);
    Task<IEnumerable<ResponseDTO>> GetAll();
    Task<bool> Delete(int id);
}