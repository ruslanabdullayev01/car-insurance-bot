namespace CarInsuranceBot.Application.IServices;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
