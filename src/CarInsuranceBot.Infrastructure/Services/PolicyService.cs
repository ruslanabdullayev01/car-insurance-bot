using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Services
{
    public sealed class PolicyService(IPolicyRepository policyRepository,
                                      IDateTimeProvider dateTimeProvider,
                                      IUnitOfWork unitOfWork) : IPolicyService
    {
        public async Task CreatePolicyAsync(string filePath, string userId)
        {
            var random = new Random();
            var policyNumber = random.Next(1000000000, int.MaxValue).ToString();
            var policy = new Policy()
            {
                PolicyNumber = policyNumber,
                UserId = userId,
                FilePath = filePath,
                CreatedDate = dateTimeProvider.UtcNow,
                IssuedAt = dateTimeProvider.UtcNow,
                ExpiryDate = dateTimeProvider.UtcNow.AddYears(1),
            };

            policyRepository.Create(policy);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
