using MoneyManager.DTOs.Auth;
using MoneyManager.DTOs.Common;

namespace MoneyManager.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<ResponseDTO>> Register(RegisterDTO registerDto);
    Task<ServiceResult<ResponseDTO>> Login(LoginDTO loginDto);
}