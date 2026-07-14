using MoneyManager.DTOs.Common;
using MoneyManager.DTOs.Expense;

namespace MoneyManager.Services.Interfaces;

public interface IExpenseService
{
    Task<ServiceResult<ResponseDto>> Create(CreateDTO createDto);
}