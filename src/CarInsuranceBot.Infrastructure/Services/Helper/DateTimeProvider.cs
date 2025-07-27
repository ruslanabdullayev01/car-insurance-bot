using CarInsuranceBot.Application.IServices.Helpers;

namespace CarInsuranceBot.Infrastructure.Services.Helper;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
