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

    public ExpenseService(IExpenseRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ServiceResult<ResponseDTO>> Create(CreateDTO createDto)
    {
        if (createDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetById(createDto.CategoryId.Value);
            if (category is null)
            {
                return ServiceResult<ResponseDTO>.NotFound(
                    $"Category with ID {createDto.CategoryId} does not exist or is deleted.");
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
        var res = new ResponseDTO()
        {
            Id = expense.Id,
            Amount = expense.Amount,
            CategoryId = expense.CategoryId,
        };
        return ServiceResult<ResponseDTO>.Success(res);
    }

    public async Task<ServiceResult<ResponseDTO>> Update(UpdateDTO updateDto, int id)
    {
        var expense = await _repository.GetById(id: id);
        if (expense is null)
            return ServiceResult<ResponseDTO>.NotFound(message: "Expense not found.");

        if (updateDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetById(id: updateDto.CategoryId.Value);
            if (category is null)
                return ServiceResult<ResponseDTO>.NotFound("Category not found.");

            expense.CategoryId = updateDto.CategoryId;
        }

        if (updateDto.Amount.HasValue)
            expense.Amount = updateDto.Amount.Value;
        expense.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChanges();

        var resDto = new ResponseDTO()
        {
            Id = expense.Id,
            Amount = expense.Amount,
            CategoryId = expense.CategoryId,
        };
        return ServiceResult<ResponseDTO>.Success(resDto);
    }

    public async Task<ServiceResult<IEnumerable<ResponseDTO>>> GetAll(QueryParameterDTO queryParameterDto)
    {
        IEnumerable<Expense> expenses = await _repository.GetAll(pageNumber: queryParameterDto.PageNumber,
            pageSize: queryParameterDto.PageSize);
        var res = expenses.Select(e => new ResponseDTO()
        {
            Id = e.Id,
            Amount = e.Amount,
        }).ToList();
        return ServiceResult<IEnumerable<ResponseDTO>>.Success(res);
    }
}