namespace MoneyManager.Models;

public class Expense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    //Navigation property
    public Category Category { get; set; } = null!;
}