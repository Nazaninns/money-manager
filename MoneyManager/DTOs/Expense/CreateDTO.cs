using System.ComponentModel.DataAnnotations;

namespace MoneyManager.DTOs.Expense;

public class CreateDTO
{
    [Required(ErrorMessage = "Amount is required")]
    [Range(1,double.MaxValue,ErrorMessage = "Amount must be a positive number")]
    public decimal Amount { get; set; }
    public int? CategoryId { get; set; }
}