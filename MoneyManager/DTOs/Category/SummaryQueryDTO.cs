using MoneyManager.Enums;

namespace MoneyManager.DTOs.Category;

public class SummaryQueryDTO
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public TimePeriod TimePeriod { get; set; } = TimePeriod.All;
    public CategorySortBy SortBy { get; set; } = CategorySortBy.Amount;
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
}