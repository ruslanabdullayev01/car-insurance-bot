namespace CarInsuranceBot.Application.IServices
{
    public interface IErrorService
    {
        Task LogErrorAsync(string message, string? stackTrace, string? userId = null, string? context = null);
    }
}
