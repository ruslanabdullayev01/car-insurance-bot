using CarInsuranceBot.Application.DTOs.User;
using CarInsuranceBot.Application.Exceptions;
using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using CarInsuranceBot.Infrastructure.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Services
{
    public class UserService(IUserRepository userRepository,
                             IDateTimeProvider dateTime,
                             IDocumentRepository documentRepository,
                             IExtractedFieldRepository extractedFieldRepository,
                             IPolicyRepository policyRepository,
                             IWebHostEnvironment webHostEnvironment,
                             IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<User> CreateOrUpdateUsersAsync(CreateOrUpdateUserRequest request, CancellationToken ct)
        {
            var user = await userRepository
                .FindByCondition(u => u.TelegramUserId == request.TelegramUserId, false)
                .FirstOrDefaultAsync(ct);
            if (user != null) return user;

            user = new User()
            {
                TelegramUserId = request.TelegramUserId,
                FullName = request.FullName,
                State = StateType.WaitingPassportPhoto,
                CreatedDate = dateTime.UtcNow
            };

            userRepository.Create(user);
            await unitOfWork.SaveChangesAsync(ct);

            return user;
        }

        public async Task<User> GetUserAsync(long telegramUserId, CancellationToken ct)
        {
            User? user = await userRepository
                .FindByCondition(u => u.TelegramUserId == telegramUserId, false)
                .FirstOrDefaultAsync(cancellationToken: ct);

            return user is null ? throw new NotFoundException("user not found") : user;
        }

        public async Task UpdateUserStateAsync(long telegramUserId, StateType newState, CancellationToken ct)
        {
            var user = await userRepository
                .FindByCondition(u => u.TelegramUserId == telegramUserId, true)
                .FirstOrDefaultAsync(cancellationToken: ct);

            if (user is not null)
            {
                user.State = newState;
                userRepository.Update(user);
                await unitOfWork.SaveChangesAsync(ct);
            }
        }

        public async Task DeleteAllDataByUserIdAsync(string userId)
        {
            var user = await userRepository.FindByCondition(u => u.Id == userId, true).Include(u => u.Documents)
                .Include(u => u.ExtractedFields).Include(u => u.Policies)
                .FirstOrDefaultAsync();
            if (user != null && user.Documents.Count != 0)
            {
                documentRepository.DeleteAddRange(user.Documents);
                foreach (var doc in user!.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.FilePath))
                        FileExtensions.DeleteFile(doc.FilePath, webHostEnvironment);
                }
            }
            if (user != null && user.ExtractedFields.Count != 0) extractedFieldRepository.DeleteAddRange(user.ExtractedFields);
            if (user != null && user.Policies.Count != 0)
            {
                policyRepository.DeleteAddRange(user.Policies);
                foreach (var policy in user.Policies)
                {
                    if (!string.IsNullOrWhiteSpace(policy.FilePath))
                        FileExtensions.DeleteFile(policy.FilePath, webHostEnvironment);
                }
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
