using CarInsuranceBot.Application.IServices.Helper;

namespace CarInsuranceBot.Infrastructure.Services.Helper;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
