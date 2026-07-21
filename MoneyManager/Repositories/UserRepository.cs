using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;

namespace MoneyManager.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task Add(User user)
    { 
        await  _dbContext.Users.AddAsync(user);
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}