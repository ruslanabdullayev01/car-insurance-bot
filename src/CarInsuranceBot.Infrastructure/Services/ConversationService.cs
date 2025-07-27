using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helpers;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Services
{
    public sealed class ConversationService(IConversationRepository conversationRepository,
                                            IDateTimeProvider dateTimeProvider,
                                            IUnitOfWork unitOfWork) : IConversationService
    {
        public async Task CreateConversationAsync(string userId, string request, string response)
        {
            var converation = new Conversation()
            {
                UserId = userId,
                Request = request,
                Response = response,
                CreatedDate = dateTimeProvider.UtcNow,
            };

            conversationRepository.Create(converation);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
