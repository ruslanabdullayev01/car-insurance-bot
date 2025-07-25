using CarInsuranceBot.Application.DTOs.User;
using Domain.Entities;
using Domain.Enums;

namespace CarInsuranceBot.Application.IServices
{
    public interface IUserService
    {
        Task<User> CreateOrUpdateUsersAsync(CreateOrUpdateUserRequest request, CancellationToken ct);
        Task<User> GetUserAsync(long telegramUserId, CancellationToken ct);
        Task UpdateUserStateAsync(long telegramUserId, StateType newState, CancellationToken ct);
    }
}
