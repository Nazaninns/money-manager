using MoneyManager.DTOs.Common;
using MoneyManager.DTOs.Expense;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpenseService(IExpenseRepository repository,  ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository =  categoryRepository;
    }

    public async Task<ServiceResult<ResponseDto>> Create(CreateDTO createDto)
    {
        if (createDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetById(createDto.CategoryId.Value);
            if (category is null)
            {
                return ServiceResult<ResponseDto>.NotFound($"Category with ID {createDto.CategoryId} does not exist or is deleted.");
            }
        }
        var expense = new Expense()
        {
            Amount = createDto.Amount,
            CategoryId = createDto.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _repository.Add(expense: expense);
        await _repository.SaveChanges();
        var res = new ResponseDto()
        {
            Id = expense.Id,
            Amount = expense.Amount,
            CategoryId = expense.CategoryId,
        };
        return ServiceResult<ResponseDto>.Success(res);
    }
}