using Microsoft.AspNetCore.Mvc;
using MoneyManager.DTOs.Auth;
using MoneyManager.DTOs.Common;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        var result = await _authService.Register(registerDto);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<ResponseDTO>.Failure(message: result.ErrorMessage));
        return Ok(ApiResponse<ResponseDTO>.Success(result.Data));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var result = await _authService.Login(loginDto);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<ResponseDTO>.Failure(message: result.ErrorMessage));
        
        return Ok(ApiResponse<ResponseDTO>.Success(result.Data));
    }
}