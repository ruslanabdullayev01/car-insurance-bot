using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Repositories
{
    public sealed class ConversationRepository(AppDbContext context) : GenericRepository<Conversation>(context), IConversationRepository
    {
    }
}
