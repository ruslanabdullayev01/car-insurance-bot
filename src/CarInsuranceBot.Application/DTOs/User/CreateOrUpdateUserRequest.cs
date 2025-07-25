namespace CarInsuranceBot.Application.DTOs.User
{
    public sealed record CreateOrUpdateUserRequest(long TelegramUserId, string? FullName);
}
