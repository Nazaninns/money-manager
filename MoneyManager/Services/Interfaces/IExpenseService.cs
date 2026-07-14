using MoneyManager.DTOs.Common;
using MoneyManager.DTOs.Expense;

namespace MoneyManager.Services.Interfaces;

public interface IExpenseService
{
    Task<ServiceResult<ResponseDTO>> Create(CreateDTO createDto);
    Task<ServiceResult<ResponseDTO>> Update(UpdateDTO updateDto , int id);
}