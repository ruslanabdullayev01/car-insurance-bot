using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Services
{
    public class ErrorService(IErrorRepository errorRepository, IUnitOfWork uow, IDateTimeProvider dateTime) : IErrorService
    {
        public async Task LogErrorAsync(string message, string? stackTrace, string? userId = null, string? context = null)
        {
            var error = new Error()
            {
                Message = message,
                StackTrace = stackTrace,
                UserId = userId,
                Context = context,
                OccurredAt = dateTime.UtcNow,
            };

            errorRepository.Create(error);
            await uow.SaveChangesAsync();
        }
    }
}
