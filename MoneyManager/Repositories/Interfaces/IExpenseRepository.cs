using MoneyManager.Models;

namespace MoneyManager.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<Expense> Add(Expense expense);
    Task SaveChanges();
    Task<Expense?> GetById(int id);
}