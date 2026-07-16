using MoneyManager.DTOs.Category;
using MoneyManager.DTOs.Common;
using MoneyManager.Models;

namespace MoneyManager.Services.Interfaces;

public interface ICategoryService
{
    Task<ServiceResult<ResponseDTO>> Create(CreateDto createDto);
    Task<ServiceResult<ResponseDTO>> GetById(int id);
    Task<ServiceResult<IEnumerable<ResponseDTO>>> GetAll();
    Task<bool> Delete(int id);
    Task<ServiceResult<ResponseDTO>> Update(UpdateDto updateDto, int id);
}