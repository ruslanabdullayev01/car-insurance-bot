using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Services
{
    public class ExtractedFieldService(IExtractedFieldRepository extractedFieldRepository,
        IUnitOfWork unitOfWork) : IExtractedFieldService
    {
        public async Task SaveExtractedFieldAsync(string userId, string key, string value)
        {
            var extractedField = new ExtractedField
            {
                UserId = userId,
                Key = key,
                Value = value
            };
            extractedFieldRepository.Create(extractedField);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetAllNonEmptyByUserIdAsync(string userId)
        {
            var fields = await extractedFieldRepository
                .FindAll(false)
                .Where(x => x.UserId == userId && !string.IsNullOrWhiteSpace(x.Value))
                .ToListAsync();

            return fields.ToDictionary(x => x.Key, x => x.Value!);
        }
    }
}
