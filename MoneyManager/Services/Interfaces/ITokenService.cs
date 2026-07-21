using MoneyManager.Models;

namespace MoneyManager.Services.Interfaces;

public interface ITokenService
{
    public string CreateToken(User user);
}