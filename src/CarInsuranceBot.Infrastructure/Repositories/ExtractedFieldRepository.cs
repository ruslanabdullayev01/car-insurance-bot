using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Repositories
{
    public class ExtractedFieldRepository(AppDbContext context) : GenericRepository<ExtractedField>(context), IExtractedFieldRepository
    {
    }
}
