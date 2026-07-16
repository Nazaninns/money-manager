namespace MoneyManager.Models;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    //Navigation property
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}