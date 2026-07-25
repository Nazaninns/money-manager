using MoneyManager.DTOs.Auth;
using MoneyManager.DTOs.Common;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;
using MoneyManager.Services.Interfaces;

namespace MoneyManager.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<ServiceResult<ResponseDTO>> Register(RegisterDTO registerDto)
    {
        var existingUser = await _userRepository.GetByEmail(registerDto.Email);
        if (existingUser is not null)
            return ServiceResult<ResponseDTO>.Failure("Email already exists");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User()
        {
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
        };

        await _userRepository.Add(user);
        await _userRepository.SaveChanges();

        string token = _tokenService.CreateToken(user);

        return ServiceResult<ResponseDTO>.Success(new ResponseDTO()
        {
            Token = token,
            Email = user.Email,
        });
    }

    public async Task<ServiceResult<ResponseDTO>> Login(LoginDTO loginDto)
    {
        var user = await _userRepository.GetByEmail(email: loginDto.Email);
        if (user is null)
            return ServiceResult<ResponseDTO>.Failure("Invalid data");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
            return ServiceResult<ResponseDTO>.Failure("Invalid data");
        
        string token = _tokenService.CreateToken(user);
        return ServiceResult<ResponseDTO>.Success(new ResponseDTO()
        {
            Token = token,
            Email = user.Email,
        });
    }
    
}