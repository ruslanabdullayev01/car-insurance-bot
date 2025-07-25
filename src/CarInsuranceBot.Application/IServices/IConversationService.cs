namespace CarInsuranceBot.Application.IServices
{
    public interface IConversationService
    {
        Task CreateConversationAsync(string userId, string request, string response);
    }
}
