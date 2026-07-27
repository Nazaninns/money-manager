namespace MoneyManager.DTOs.Category;

public class SummaryDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int ExpenseCount { get; set; }
}