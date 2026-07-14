namespace MoneyManager.DTOs.Expense;

public class ResponseDto
{
   public int Id { get; set; }
   public decimal Amount { get; set; }
   public int? CategoryId { get; set; }
}