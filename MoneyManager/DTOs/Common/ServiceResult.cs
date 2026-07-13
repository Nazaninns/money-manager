namespace MoneyManager.DTOs.Common;

public class ServiceResult
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool IsNotFound { get; private set; }

    public static ServiceResult Success() => new() { IsSuccess = true };
    public static ServiceResult Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
    public static ServiceResult NotFound(string message) => new() { IsSuccess = false, ErrorMessage = message, IsNotFound = true };
}