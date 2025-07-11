namespace Domain.Abstractions;
public class Result<T>
{
    public T data { get; set; }
    public int statusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> errors { get; set; }

    public static Result<T> Success(int statusCode, T _data) => new Result<T>
    {
        statusCode = statusCode,
        IsSuccess = true,
        data = _data
    };
    public static Result<T> Success(int _statusCode) => new Result<T> { statusCode = _statusCode, IsSuccess = true };
    public static Result<T> Fail(int statusCode, List<string> errors) => new Result<T>
    {
        statusCode = statusCode,
        errors = errors,
        IsSuccess = false
    };
    public static Result<T> Fail(int _statusCode, string error) => new Result<T>
    {
        statusCode = _statusCode,
        IsSuccess = false,
        errors = new List<string> { error }
    };

}
