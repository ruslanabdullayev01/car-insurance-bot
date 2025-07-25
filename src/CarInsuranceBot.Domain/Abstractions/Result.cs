namespace Domain.Abstractions;
public class Result<T>
{
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string>? Errors { get; set; }

    public static Result<T> Success(int statusCode, T _data) => new()
    {
        StatusCode = statusCode,
        IsSuccess = true,
        Data = _data
    };
    public static Result<T> Success(int _statusCode) => new() { StatusCode = _statusCode, IsSuccess = true };
    public static Result<T> Fail(int statusCode, List<string> errors) => new()
    {
        StatusCode = statusCode,
        Errors = errors,
        IsSuccess = false
    };
    public static Result<T> Fail(int _statusCode, string error) => new()
    {
        StatusCode = _statusCode,
        IsSuccess = false,
        Errors = [error]
    };

}
