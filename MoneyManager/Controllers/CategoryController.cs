using Microsoft.AspNetCore.Mvc;
using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.DTOs.Common;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    //Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateDto dto)
    {
        var category = await _categoryService.Create(dto);
        var response = ApiResponse<ResponseDTO>.Success(data: category, message: "Category created successfully");
        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = category.Id },
            value: response
        );
    }

    //Get by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetById(id);
        if (category is null)
            return NotFound(ApiResponse<object>.Failure(message: "Category not found"));

        return Ok(ApiResponse<ResponseDTO>.Success(data: category));
    }
}