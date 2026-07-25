using MoneyManager.Models;

namespace MoneyManager.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByEmail(string email);
    public Task Add(User user);
    public Task SaveChanges();
}