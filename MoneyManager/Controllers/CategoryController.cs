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
        var result = await _categoryService.Create(dto);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<object>.Failure(result.ErrorMessage));
        var response = ApiResponse<ResponseDTO>.Success(data: result.Data, message: "Category created successfully");
        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = result.Data!.Id },
            value: response
        );
    }

    //Update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateDto dto)
    {
        var result = await _categoryService.Update(dto, id);
        if (!result.IsSuccess)
        {
            if (result.IsNotFound)
                return NotFound(ApiResponse<object>.Failure(message: result.ErrorMessage));

            return BadRequest(ApiResponse<object>.Failure(message: result.ErrorMessage));
        }

        return Ok(ApiResponse<object>.Success(data: result.Data, message: "Category updated successfully"));
    }

    //Get by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetById(id);
        if (!result.IsSuccess)
            return NotFound(ApiResponse<object>.Failure(message: result.ErrorMessage));

        return Ok(ApiResponse<ResponseDTO>.Success(data: result.Data));
    }

    //Get all
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAll();
        return Ok(ApiResponse<IEnumerable<ResponseDTO>>.Success(data: result.Data));
    }

    //Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool success = await _categoryService.Delete(id);
        if (!success)
            return NotFound(ApiResponse<object>.Failure(message: "Category not found or already deleted."));
        return Ok(ApiResponse<object>.Success(data: null, message: "Category deleted successfully"));
    }
}