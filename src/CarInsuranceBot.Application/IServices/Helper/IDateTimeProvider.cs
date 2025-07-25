namespace CarInsuranceBot.Application.IServices.Helper;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
