namespace CarInsuranceBot.Application.IServices.Helpers;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
