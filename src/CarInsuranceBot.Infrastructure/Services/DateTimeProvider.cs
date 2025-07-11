using CarInsuranceBot.Application.IServices;

namespace CarInsuranceBot.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
