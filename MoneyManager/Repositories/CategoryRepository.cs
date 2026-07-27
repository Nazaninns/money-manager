using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.DTOs.Category;
using MoneyManager.Enums;
using MoneyManager.Models;
using MoneyManager.Repositories.Interfaces;

namespace MoneyManager.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Category> Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        return category;
    }

    public async Task<IEnumerable<Category>> GetAll(int userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId || c.UserId == null)
            .Where(c => c.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<Category?> GetById(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByTitle(string title)
    {
        return await _context.Categories.AnyAsync(c =>
            EF.Functions.Like(c.Title, title) && c.DeletedAt == null);
    }

    public async Task<IEnumerable<SummaryDTO>> GetSummary(int userId, SummaryQueryDTO summaryQueryDto)
    {
        DateTime? startDate = summaryQueryDto.TimePeriod switch
        {
            TimePeriod.LastWeek => DateTime.UtcNow.AddDays(-7),
            TimePeriod.LastMonth => DateTime.UtcNow.AddMonths(-1),
            TimePeriod.LastYear => DateTime.UtcNow.AddYears(-1),
            _ => null
        };
        var query = _context.Categories
            .Where(c => c.UserId == userId || c.UserId == null)
            .Select(c => new SummaryDTO
            {
                Id = c.Id,
                Title = c.Title,

                TotalAmount = c.Expenses
                    .Where(e => e.UserId == userId && (startDate == null || e.CreatedAt >= startDate))
                    .Sum(e => (decimal?)e.Amount) ?? 0,

                ExpenseCount =
                    c.Expenses.Count(e => e.UserId == userId && (startDate == null || e.CreatedAt >= startDate))
            });

        query = (summaryQueryDto.SortBy, summaryQueryDto.SortDirection) switch
        {
            (CategorySortBy.Count, SortDirection.Ascending)
                => query.OrderBy(c => c.ExpenseCount),
            (CategorySortBy.Count, SortDirection.Descending)
                => query.OrderByDescending(c => c.ExpenseCount),
            (CategorySortBy.Amount, SortDirection.Ascending)
                => query.OrderBy(c => c.TotalAmount),
            _ => query.OrderByDescending(c => c.TotalAmount)
        };

        return await query
            .Skip((summaryQueryDto.PageNumber - 1) * summaryQueryDto.PageSize)
            .Take(summaryQueryDto.PageSize)
            .ToListAsync();
    }
}