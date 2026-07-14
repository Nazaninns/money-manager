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

        var response = ApiResponse<ResponseDto>.Success(data: result.Data, message: "Expense created successfully");
        return CreatedAtAction(
            "GetAll",
            routeValues: new { id = result.Data!.Id },
            value: response
        );
    }
}