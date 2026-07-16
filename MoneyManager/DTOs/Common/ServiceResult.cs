namespace MoneyManager.DTOs.Common;

public class ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool IsNotFound { get; private set; }

    public T? Data { get; private set; }

    public static ServiceResult<T> Success(T? data) => new() { IsSuccess = true , Data =  data };
    public static ServiceResult<T> Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };

    public static ServiceResult<T> NotFound(string message) =>
        new() { IsSuccess = false, ErrorMessage = message, IsNotFound = true };
}