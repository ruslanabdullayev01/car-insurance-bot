using CarInsuranceBot.Application.DTOs.User;
using CarInsuranceBot.Application.Exceptions;
using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Services
{
    public class UserService(IUserRepository userRepository,
                             IDateTimeProvider dateTime,
                             IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<User> CreateOrUpdateUsersAsync(CreateOrUpdateUserRequest request, CancellationToken ct)
        {
            var user = await userRepository
                .FindByCondition(u => u.TelegramUserId == request.TelegramUserId, false)
                .FirstOrDefaultAsync(ct);
            if (user != null) return user;

            user = new Domain.Entities.User()
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
    }
}
