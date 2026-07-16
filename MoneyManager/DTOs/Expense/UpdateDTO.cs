using System.ComponentModel.DataAnnotations;

namespace MoneyManager.DTOs.Expense;

public class UpdateDTO
{
    [Range(1,double.MaxValue,ErrorMessage = "Category must be a positive number")]
    public decimal? Amount { get; set; }
    public int? CategoryId { get; set; }
}