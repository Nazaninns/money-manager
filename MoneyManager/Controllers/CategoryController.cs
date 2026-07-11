using Microsoft.AspNetCore.Mvc;
using MoneyManager.Data;
using MoneyManager.DTOs.Category;
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
        var category = await _categoryService.CreateCategory(dto);
        return CreatedAtAction(nameof(Create), new { id = category.Id }, category);
        
    }
}