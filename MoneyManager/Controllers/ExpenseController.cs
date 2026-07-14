using Microsoft.AspNetCore.Mvc;
using MoneyManager.DTOs.Common;
using MoneyManager.DTOs.Expense;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDTO createDto)
    {
        var result = await _expenseService.Create(createDto: createDto);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<object>.Failure(result.ErrorMessage));

        var response = ApiResponse<ResponseDTO>.Success(data: result.Data, message: "Expense created successfully");
        return CreatedAtAction(
            "GetAll",
            routeValues: new { id = result.Data!.Id },
            value: response
        );
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDTO updateDto)
    {
        var result = await _expenseService.Update(updateDto: updateDto, id: id);
        if (!result.IsSuccess)
        {
            if (result.IsNotFound)
                return NotFound(ApiResponse<object>.Failure(result.ErrorMessage));
            return BadRequest(ApiResponse<object>.Failure(result.ErrorMessage));
        }

        return Ok(ApiResponse<ResponseDTO>.Success(data: result.Data, message: "Expense updated successfully"));
    }
}