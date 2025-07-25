using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Repositories
{
    public sealed class DocumentRepository(AppDbContext context) : GenericRepository<Document>(context), IDocumentRepository
    {
    }
}
