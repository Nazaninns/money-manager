using MoneyManager.Data;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;

namespace MoneyManager.Repositories;

public class ExpenseRepository: IExpenseRepository
{
    private readonly AppDbContext _context;

    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Expense> Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        return expense;
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Expense?> GetById(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }
}